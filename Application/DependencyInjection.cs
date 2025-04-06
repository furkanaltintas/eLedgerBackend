using Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        services.AddMapster();

        services.AddMediatR(configuration =>
        { configuration.RegisterServicesFromAssemblies(assembly, typeof(AppUser).Assembly); });

        return services;
    }
}