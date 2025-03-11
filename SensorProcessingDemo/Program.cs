using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SensorProcessingDemo.Auth;
using SensorProcessingDemo.Repositories.Implementations;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;
using SensorProcessingDemo.Services.Implementations;
using SensorProcessingDemo.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMonitoringService, MonitoringService>();

var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

builder.Services.AddApiAuthAuthentification(configuration, jwtOptions);
builder.Services.AddDbContextFactory<MonitoringSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{    
    app.UseHsts();
}

var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
appLifetime.ApplicationStopping.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var monitoringService = scope.ServiceProvider.GetRequiredService<IMonitoringService>();
    var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

    int userId = Convert.ToInt32(currentUserService.GetUserId());
    await monitoringService.StopMonitoring(userId);
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
