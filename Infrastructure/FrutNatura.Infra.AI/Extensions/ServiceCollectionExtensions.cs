using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.AI.Options;

namespace FrutNatura.Infra.AI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAi(this IServiceCollection services, IConfiguration cfg)
    {
        // Lê a seção "AI"
        services.Configure<AiOptions>(cfg.GetSection("AI"));

        // Registra HttpClient (usado no OpenAI)
        services.AddHttpClient();

        // Decide o provider com base no appsettings
        services.AddScoped<IAIProvider>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<AiOptions>>().Value;
            var prov = (opt.Provider ?? "Echo").Trim().ToLowerInvariant();

            return prov switch
            {
                "openai" => new OpenAiProvider(sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<IOptions<AiOptions>>()),
                _ => new EchoAiProvider(),
            };
        });

        return services;
    }
}
