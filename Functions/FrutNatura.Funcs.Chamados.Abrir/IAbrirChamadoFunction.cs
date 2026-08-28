using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.Funcs.Chamados.Abrir
{
    public interface IAbrirChamadoFunction
    {
        // clienteId vem do usuário autenticado
        Task<int> ExecuteAsync(int clienteId, AbrirChamadoRequest request, CancellationToken ct = default);
    }
}
