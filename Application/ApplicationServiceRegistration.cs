using Application.Common.Mapping;
using Application.Features.Banks.Rules;
using Domain.Entities.Partners;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

        services.AddScoped<BankRules>();

        return services;
    }
}