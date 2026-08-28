using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Funcs.Mensagens.Enviar;

public sealed class EnviarMensagemFunction
{
    private readonly IMensagensService _mensagens;
    public EnviarMensagemFunction(IMensagensService mensagens) => _mensagens = mensagens;

    public Task<Guid> HandleAsync(Guid chamadoId, string texto, Guid? autorId, CancellationToken ct = default)
        => _mensagens.EnviarAsync(chamadoId, texto, autorId, ct);
}
