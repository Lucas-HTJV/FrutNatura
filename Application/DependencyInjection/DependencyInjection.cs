using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistence(config);
        services.AddSecurity(config);
        services.AddEmail(config);
        services.AddAI(config);
        services.AddRealtime(config);
        return services;
    }
}
