using System.Reflection;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.Email.Options;
using FrutNatura.Core.Abstractions.Repositories;

namespace FrutNatura.Infra.Email.Providers;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly EmailOptions _options;
    private readonly IEmailTemplateRenderer _template;
    private readonly IUsuariosRepository _usuarios;

    public SmtpEmailSender(
        IOptions<EmailOptions> options,
        IEmailTemplateRenderer template,
        IUsuariosRepository usuarios)
    {
        _options = options.Value;
        _template = template;
        _usuarios = usuarios;
    }

    // Assinatura EXATA exigida pela interface
    public async Task SendAsync(Guid destinatarioId, string subject, string htmlBody, CancellationToken ct = default)
    {
        var to = await ResolveEmailAsync(destinatarioId, ct);
        if (string.IsNullOrWhiteSpace(to))
            throw new InvalidOperationException("E-mail do destinatário não encontrado.");

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl, ct);
        if (!string.IsNullOrWhiteSpace(_options.User))
            await client.AuthenticateAsync(_options.User, _options.Password, ct);

        await client.SendAsync(msg, ct);
        await client.DisconnectAsync(true, ct);
    }

    /// <summary>
    /// Tenta invocar no repositório um método async que busque o usuário por Id
    /// (ObterPorIdAsync, GetByIdAsync, FindByIdAsync, GetAsync) e extrai o e-mail.
    /// </summary>
    private async Task<string?> ResolveEmailAsync(Guid id, CancellationToken ct)
    {
        var repoType = _usuarios.GetType();

        // possíveis nomes de métodos no seu repositório
        var methodNames = new[] { "ObterPorIdAsync", "GetByIdAsync", "FindByIdAsync", "GetAsync" };

        MethodInfo? m = null;
        foreach (var name in methodNames)
        {
            m = repoType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);
            if (m != null) break;
        }

        if (m == null)
            throw new MissingMethodException($"{repoType.Name} não expõe método de busca por Id (ex.: ObterPorIdAsync/GetByIdAsync).");

        // suporta (Guid id) ou (Guid id, CancellationToken ct)
        var parameters = m.GetParameters();
        object? result;
        if (parameters.Length == 1)
            result = m.Invoke(_usuarios, new object?[] { id });
        else if (parameters.Length == 2 && parameters[1].ParameterType == typeof(CancellationToken))
            result = m.Invoke(_usuarios, new object?[] { id, ct });
        else
            throw new MissingMethodException($"{repoType.Name}.{m.Name} tem assinatura inesperada.");

        // se retornou Task/Task<T>, aguarda e extrai o valor
        if (result is Task task)
        {
            await task.ConfigureAwait(false);
            var taskType = task.GetType();
            if (taskType.IsGenericType)
            {
                // Task<T>: pega Result
                result = taskType.GetProperty("Result")?.GetValue(task);
            }
            else
            {
                // Task sem retorno -> não sabemos obter o usuário
                result = null;
            }
        }

        if (result is null) return null;

        // tenta ler propriedades comuns de e-mail
        var email =
            TryGetStringProp(result, "Email") ??
            TryGetStringProp(result, "EmailAddress") ??
            TryGetNestedStringProp(result, "Contato", "Email");

        return email;
    }

    private static string? TryGetStringProp(object obj, string prop)
        => obj.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public)?.GetValue(obj)?.ToString();

    private static string? TryGetNestedStringProp(object obj, string parent, string child)
    {
        var p = obj.GetType().GetProperty(parent, BindingFlags.Instance | BindingFlags.Public)?.GetValue(obj);
        if (p is null) return null;
        return p.GetType().GetProperty(child, BindingFlags.Instance | BindingFlags.Public)?.GetValue(p)?.ToString();
    }
}
