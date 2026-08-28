using Microsoft.AspNetCore.SignalR;
using FrutNatura.Core.Abstractions.Notifications;
using FrutNatura.Infra.Realtime.Hubs;

namespace FrutNatura.Infra.Realtime;

public sealed class SignalRRealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<ChamadosHub> _hub;

    public SignalRRealtimeNotifier(IHubContext<ChamadosHub> hub) => _hub = hub;

    public Task NotifyChamadoAbertoAsync(Guid chamadoId, Guid clienteId, CancellationToken ct = default)
        => _hub.Clients.Group($"cliente:{clienteId:D}")
               .SendAsync(ChamadosHub.EventChamadoAberto, new { chamadoId, clienteId }, ct);

    public Task NotifyStatusAlteradoAsync(Guid chamadoId, string novoStatus, CancellationToken ct = default)
        => _hub.Clients.Group($"chamado:{chamadoId:D}")
               .SendAsync(ChamadosHub.EventStatusAlterado, new { chamadoId, novoStatus }, ct);

    public Task NotifyChamadoAtualizadoAsync(Guid chamadoId, CancellationToken ct = default)
        => _hub.Clients.Group($"chamado:{chamadoId:D}")
               .SendAsync(ChamadosHub.EventChamadoAtualizado, new { chamadoId }, ct);

    public Task NotifyMensagemEnviadaAsync(Guid chamadoId, Guid mensagemId, CancellationToken ct = default)
        => _hub.Clients.Group($"chamado:{chamadoId:D}")
               .SendAsync(ChamadosHub.EventMensagemEnviada, new { chamadoId, mensagemId }, ct);
}
