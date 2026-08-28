using MediatR;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;

namespace FrutNatura.App.Application.UseCases.Chamados.AtribuirChamado;


public sealed class AtribuirChamadoHandler : IRequestHandler<AtribuirChamadoCommand, Unit>
{
    private readonly IChamadosRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IRealtimeNotifier _notifier;

    public AtribuirChamadoHandler(IChamadosRepository repo, IUnitOfWork uow, IRealtimeNotifier notifier)
    {
        _repo = repo;
        _uow = uow;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(AtribuirChamadoCommand request, CancellationToken cancellationToken)
    {
        var chamado = await _repo.GetByIdAsync(request.ChamadoId, cancellationToken);
        if (chamado is null)
            throw new InvalidOperationException("Chamado não encontrado.");

        chamado.AtribuirResponsavel(request.ResponsavelId);

        await _repo.UpdateAsync(chamado, cancellationToken);
        

        await _notifier.NotifyChamadoAtualizadoAsync(chamado.Id, cancellationToken);
        return Unit.Value;
    }
}
