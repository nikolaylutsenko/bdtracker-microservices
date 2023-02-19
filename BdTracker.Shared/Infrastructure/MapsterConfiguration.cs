using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BdTracker.Shared.Infrastructure;

public static class MapsterConfiguration
{
    public static void AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        var applicationAssembly = services.GetType().Assembly;
        config.Scan(applicationAssembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}