using FrutNatura.App.Application.Common;
using MediatR;

namespace FrutNatura.App.Application.UseCases.Chamados.AtribuirChamado;

public sealed class AtribuirChamadoCommand : IRequest<Unit>, ITransactionalRequest
{
    public Guid ChamadoId { get; init; }
    public Guid ResponsavelId { get; init; }

    public AtribuirChamadoCommand() { }

    public AtribuirChamadoCommand(Guid chamadoId, Guid responsavelId)
    {
        ChamadoId = chamadoId;
        ResponsavelId = responsavelId;
    }
}
