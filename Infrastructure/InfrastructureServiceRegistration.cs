using Application.Common.Interfaces;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Context;
using Infrastructure.Services.Jwt;
using Infrastructure.SignalR.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Redis ayarı
        // services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICompanyContextHelper, CompanyContextHelper>();
        services.AddScoped<ISignalRService, SignalRService>();
        services.AddSignalR();

        return services;
    }
}