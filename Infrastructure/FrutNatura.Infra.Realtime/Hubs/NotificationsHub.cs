using Microsoft.AspNetCore.SignalR;

namespace FrutNatura.Infra.Realtime.Hubs;

/// <summary>
/// Hub central de notificações. Os clientes chamam Join*/Leave* para entrarem em grupos.
/// </summary>
public class NotificationsHub : Hub
{
    // Convenções de nomes de grupos
    public static string GroupCliente(Guid clienteId) => $"cliente-{clienteId:N}";
    public static string GroupChamado(Guid chamadoId) => $"chamado-{chamadoId:N}";
    public const string GroupStaff = "staff";

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        // Você pode logar conexão aqui.
    }

    // ====== Métodos de assinatura de grupos ======

    public Task JoinCliente(Guid clienteId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GroupCliente(clienteId));

    public Task LeaveCliente(Guid clienteId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupCliente(clienteId));

    public Task JoinChamado(Guid chamadoId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GroupChamado(chamadoId));

    public Task LeaveChamado(Guid chamadoId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupChamado(chamadoId));

    public Task JoinStaff()
        => Groups.AddToGroupAsync(Context.ConnectionId, GroupStaff);

    public Task LeaveStaff()
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupStaff);
}
