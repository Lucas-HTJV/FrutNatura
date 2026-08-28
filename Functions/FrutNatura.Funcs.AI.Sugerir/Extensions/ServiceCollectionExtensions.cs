using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.AI.Sugerir;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSugerirFunc(this IServiceCollection services)
    {
        services.AddScoped<SugerirRespostaFunction>();
        // se tiver interface: services.AddScoped<ISugerirRespostaFunction, SugerirRespostaFunction>();
        return services;
    }
}
