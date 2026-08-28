using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Chamados;
using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Abstractions.Services
{
    public interface IChamadosService
    {
        
        Task<Guid> AbrirAsync(Guid clienteId, string titulo, string descricao, CancellationToken ct = default);

     
        Task AtribuirAsync(Guid chamadoId, Guid? responsavelId, CancellationToken ct = default);

       
        Task<PagedResult<ChamadoListDto>> ListarDoClienteAsync(Guid clienteId, int page, int pageSize, CancellationToken ct = default);

    
        Task<Chamado?> ObterChamadoAsync(Guid id, CancellationToken ct);
        Task AtualizarChamadoAsync(Chamado chamado, CancellationToken ct);
    }
}
