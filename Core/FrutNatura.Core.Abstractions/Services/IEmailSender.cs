namespace FrutNatura.Core.Abstractions.Services;

public interface IEmailSender
{
    // Envia para um usuário/cliente identificado por Id
    Task SendAsync(Guid destinatarioId, string subject, string htmlBody, CancellationToken ct = default);
}
