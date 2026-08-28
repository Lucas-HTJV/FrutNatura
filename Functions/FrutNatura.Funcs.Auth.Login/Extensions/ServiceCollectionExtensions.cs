using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Funcs.Auth.Login;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoginFunc(this IServiceCollection services)
    {
        // sem interface:
        services.AddScoped<LoginFunction>();

        // ou com interface ILoginFunction:
        // services.AddScoped<ILoginFunction, LoginFunction>();

        return services;
    }
}
