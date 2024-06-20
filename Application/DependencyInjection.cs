using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // register automapper service
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());

            //cfg.AddMaps(new[]
            //{
            //    "Application",
            //    "Infrastructure"
            //});
        });

        // register mediator service
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
