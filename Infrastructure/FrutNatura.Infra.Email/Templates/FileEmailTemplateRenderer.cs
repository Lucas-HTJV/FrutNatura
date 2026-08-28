using FrutNatura.Core.Abstractions.Services;
using System.IO;

namespace FrutNatura.Infra.Email.Providers;

public sealed class FileEmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly string _templatesRoot;

    public FileEmailTemplateRenderer(string templatesRoot = "Templates")
        => _templatesRoot = templatesRoot;

    public string Render(string templateKey, object model)
    {
        // Implementação simplificada: lê o arquivo e retorna o conteúdo
        // (depois você pode trocar por Razor ou outra engine)
        var path = Path.Combine(_templatesRoot, $"{templateKey}.html");
        return File.Exists(path) ? File.ReadAllText(path) : $"<html><body>{templateKey}</body></html>";
    }
}
