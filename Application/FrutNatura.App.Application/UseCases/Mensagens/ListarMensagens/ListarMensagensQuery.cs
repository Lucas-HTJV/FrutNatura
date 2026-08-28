using MediatR;
using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Mensagens;

namespace FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens;

public sealed class ListarMensagensQuery : IRequest<IReadOnlyList<MensagemDto>>
{
    public Guid ChamadoId { get; init; }
}
