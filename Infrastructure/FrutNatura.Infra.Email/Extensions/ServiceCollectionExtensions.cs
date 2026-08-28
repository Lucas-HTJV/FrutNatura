using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.Email.Options;
using FrutNatura.Infra.Email.Providers;

namespace FrutNatura.Infra.Email.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        // Escolha UM renderer: FileEmailTemplateRenderer OU RazorEmailTemplateRenderer
        services.AddScoped<IEmailTemplateRenderer>(_ => new FileEmailTemplateRenderer("Templates"));
        // services.AddScoped<IEmailTemplateRenderer, RazorEmailTemplateRenderer>();

        services.AddScoped<IEmailSender, SmtpEmailSender>();

        return services;
    }
}
