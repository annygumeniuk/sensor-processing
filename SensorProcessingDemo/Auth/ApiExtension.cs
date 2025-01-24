using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SensorProcessingDemo.Auth
{
    public static class ApiExtension
    {
        public static void AddApiAuthAuthentification(
            this 
            IServiceCollection services,
            IConfiguration configuration,
            IOptions<JwtOptions> jwtOptions)
        {
            var jwtSettings = jwtOptions.Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty-cookies"];

                            return Task.CompletedTask;
                        }
                    };
                }).AddCookie("Cookies", options =>
                {
                    options.Cookie.Name = "tasty-cookies";
                    options.SlidingExpiration = true;
                });

            services.AddAuthorization();
        }
    }
}