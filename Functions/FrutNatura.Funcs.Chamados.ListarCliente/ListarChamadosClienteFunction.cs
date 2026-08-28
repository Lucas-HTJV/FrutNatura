using FrutNatura.Core.Abstractions.Common.PageResults;     // PagedResult<>
using FrutNatura.Core.Abstractions.Services;   // IChamadosService
using FrutNatura.Core.Contracts.Chamados;      // ChamadoListDto

namespace FrutNatura.Funcs.Chamados.ListarCliente;

public sealed class ListarChamadosClienteFunction
{
    private readonly IChamadosService _chamados;
    public ListarChamadosClienteFunction(IChamadosService chamados) => _chamados = chamados;

    public Task<PagedResult<ChamadoListDto>> HandleAsync(Guid clienteId, int page, int pageSize, CancellationToken ct = default)
        => _chamados.ListarDoClienteAsync(clienteId, page, pageSize, ct);
}
