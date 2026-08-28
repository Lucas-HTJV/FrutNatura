using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Auth.Register;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegisterFunc(this IServiceCollection services)
    {
        services.AddScoped<RegisterFunction>(); // ✅ NÃO use dois genéricos aqui
        return services;
    }
}
