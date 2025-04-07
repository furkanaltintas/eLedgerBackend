using Infrastructure.Services.Cache;
using Infrastructure.Services.Jwt;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Redis ayarı
        // services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));


        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}