using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Chamados.ListarCliente;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddListarChamadosClienteFunc(this IServiceCollection services)
    {
        services.AddScoped<ListarChamadosClienteFunction>(); // <- nome correto da classe
        return services;
    }
}
