using Mapster;
using MapsterMapper;

namespace BdTracker.Groups.Infrastructure;

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