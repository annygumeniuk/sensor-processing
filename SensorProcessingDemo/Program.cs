using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SensorProcessingDemo.Auth;
using SensorProcessingDemo.Repositories.Implementations;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;
using SensorProcessingDemo.Services.Implementations;
using SensorProcessingDemo.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMonitoringService, MonitoringService>();

var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

builder.Services.AddApiAuthAuthentification(configuration, jwtOptions);
builder.Services.AddDbContext<MonitoringSystemContext>(options =>
    options.UseSqlServer("Server=(local)\\sqlexpress;Database=MonitoringSystem;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
