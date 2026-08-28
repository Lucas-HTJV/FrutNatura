using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Chamados.Atribuir;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAtribuirChamadoFunc(this IServiceCollection services)
    {
        services.AddScoped<AtribuirChamadoFunction>(); // ✅
        // Se tiver interface: services.AddScoped<IAtribuirChamadoFunction, AtribuirChamadoFunction>();
        return services;
    }
}
