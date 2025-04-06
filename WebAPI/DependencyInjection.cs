using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI;

public static class DependencyInjection
{
    public const string AllowSpecificOrigins = "AllowSpecificOrigins";

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetSection("JwtSettings:Issuer").Value,
                ValidAudience = configuration.GetSection("JwtSettings:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SecretKey").Value ?? string.Empty))
            };
        });

        services.AddAuthorizationBuilder();

        services.AddCors(options =>
        {
            options.AddPolicy(AllowSpecificOrigins, policy =>
            {
                policy.WithOrigins("http://localhost:4200") // Güvenilir domainleri ekle           
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithExposedHeaders("Authorization");
            });
        });

        return services;
    }
}
