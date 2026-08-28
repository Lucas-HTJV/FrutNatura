using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FrutNatura.Infra.Realtime.Hubs;

[Authorize] // remova se não usar auth
public sealed class ChamadosHub : Hub
{
    public const string EventChamadoAberto = "ChamadoAberto";
    public const string EventChamadoAtualizado = "ChamadoAtualizado";
    public const string EventStatusAlterado = "StatusAlterado";
    public const string EventMensagemEnviada = "MensagemEnviada";

    private static string GrupoCliente(Guid clienteId) => $"cliente:{clienteId:D}";
    private static string GrupoChamado(Guid chamadoId) => $"chamado:{chamadoId:D}";

    public Task JoinCliente(Guid clienteId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GrupoCliente(clienteId));

    public Task LeaveCliente(Guid clienteId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoCliente(clienteId));

    public Task JoinChamado(Guid chamadoId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GrupoChamado(chamadoId));

    public Task LeaveChamado(Guid chamadoId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoChamado(chamadoId));
}
