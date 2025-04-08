using Application.Mapping;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        #region Mapper
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(SourceToDestinationMapping).Assembly); // Mapping class'larını tara
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        #endregion

        #region MediatR
        services.AddMediatR(configuration =>
        { configuration.RegisterServicesFromAssemblies(assembly, typeof(AppUser).Assembly); });
        #endregion

        return services;
    }
}