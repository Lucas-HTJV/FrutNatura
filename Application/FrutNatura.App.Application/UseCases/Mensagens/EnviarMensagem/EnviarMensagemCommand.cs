using FrutNatura.App.Application.Common;

namespace FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;
public sealed class EnviarMensagemCommand(
    Guid chamadoId,
    Guid? autorId,
    string conteudo
) : IRequest<Guid>, ITransactionalRequest
{
    public Guid ChamadoId { get; } = chamadoId;
    public Guid? AutorId { get; } = autorId;
    public string Conteudo { get; } = conteudo;
}
