using FrutNatura.App.Application.Common;
using FrutNatura.Core.Domain.Enums;
using MediatR;

namespace FrutNatura.App.Application.UseCases.Chamados.AbrirChamado;

public sealed class AbrirChamadoCommand : IRequest<Guid>, ITransactionalRequest
{
    public Guid ClienteId { get; init; }
    public string Titulo { get; init; } = default!;
    public string Descricao { get; init; } = default!;
    public Prioridade Prioridade { get; init; } = Prioridade.Normal;
}
