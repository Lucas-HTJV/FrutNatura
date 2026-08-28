namespace FrutNatura.Core.Abstractions.Services;

public interface IEmailTemplateRenderer
{
    /// Renderiza o template identificado por key com o model fornecido.
    string Render(string templateKey, object model);
}
