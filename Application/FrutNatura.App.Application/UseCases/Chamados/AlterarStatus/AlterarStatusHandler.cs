using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.Chamados.AlterarStatus;


public sealed class AlterarStatusHandler : IRequestHandler<AlterarStatusCommand, bool>
{
    private readonly IChamadosRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IRealtimeNotifier _notifier;

    public AlterarStatusHandler(IChamadosRepository repo, IUnitOfWork uow, IRealtimeNotifier notifier)
    {
        _repo = repo;
        _uow = uow;
        _notifier = notifier;
    }

    public async Task<bool> Handle(AlterarStatusCommand request, CancellationToken cancellationToken)
    {
        var chamado = await _repo.GetByIdAsync(request.ChamadoId, cancellationToken);
        if (chamado is null) return false;

        chamado.AlterarStatus(request.NovoStatus);

        await _repo.UpdateAsync(chamado, cancellationToken);
      

        await _notifier.NotifyStatusAlteradoAsync(chamado.Id, request.NovoStatus.ToString(), cancellationToken);
        await _notifier.NotifyChamadoAtualizadoAsync(chamado.Id, cancellationToken);
        return true;
    }
}
