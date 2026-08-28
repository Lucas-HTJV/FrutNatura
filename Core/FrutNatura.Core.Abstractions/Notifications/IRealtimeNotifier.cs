namespace FrutNatura.Core.Abstractions.Notifications;

public interface IRealtimeNotifier
{
    Task NotifyChamadoAbertoAsync(Guid chamadoId, Guid clienteId, CancellationToken ct = default);
    Task NotifyStatusAlteradoAsync(Guid chamadoId, string novoStatus, CancellationToken ct = default);

   
    Task NotifyChamadoAtualizadoAsync(Guid chamadoId, CancellationToken ct = default);
    Task NotifyMensagemEnviadaAsync(Guid chamadoId, Guid mensagemId, CancellationToken ct = default);
}
