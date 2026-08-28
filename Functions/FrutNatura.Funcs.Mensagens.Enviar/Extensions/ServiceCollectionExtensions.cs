using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Mensagens.Enviar;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnviarMensagemFunc(this IServiceCollection services)
    {
        services.AddScoped<EnviarMensagemFunction>(); // ✅
        // (Se você tiver uma interface, use: services.AddScoped<IEnviarMensagemFunction, EnviarMensagemFunction>();)
        return services;
    }
}
