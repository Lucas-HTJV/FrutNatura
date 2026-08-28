using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Infra.Email.Providers;

public sealed class RazorEmailTemplateRenderer : IEmailTemplateRenderer
{
    public string Render(string templateKey, object model)
        => $"<html><body><h3>{templateKey}</h3><pre>{model}</pre></body></html>";
}
