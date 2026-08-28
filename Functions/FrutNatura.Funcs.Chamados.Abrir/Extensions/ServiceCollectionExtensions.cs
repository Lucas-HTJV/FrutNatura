using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Chamados.Abrir;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAbrirChamadoFunc(this IServiceCollection services)
    {
        services.AddScoped<AbrirChamadoFunction>();
        return services;
    }
}
