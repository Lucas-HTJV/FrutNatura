using FrutNatura.Core.Abstractions.Common.PageResults;     // PagedResult<>
using FrutNatura.Core.Abstractions.Services;   // IChamadosService
using FrutNatura.Core.Contracts.Chamados;      // ChamadoListDto
namespace FrutNatura.Funcs.Chamados.ListarCliente
{
    public interface IListarClienteFunction
    {
        Task<PagedResult<ChamadoListDto>> ExecuteAsync(int clienteId, int page, int pageSize, CancellationToken ct = default);
    }
}
