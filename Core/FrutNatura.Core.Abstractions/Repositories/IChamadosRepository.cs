using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Abstractions.Repositories
{
    public interface IChamadosRepository
    {
        Task<Chamado?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Chamado>> ListAsync(ISpecification<Chamado> spec, CancellationToken ct = default);

        Task<int> CountAsync(ISpecification<Chamado> spec,CancellationToken ct = default);

        Task AddAsync(Chamado entity, CancellationToken ct = default);

        Task UpdateAsync(Chamado entity, CancellationToken ct = default);

       
        Task AbrirAsync(Chamado chamado, CancellationToken ct = default);

        Task AtribuirResponsavelAsync(Guid chamadoId,Guid responsavelId,CancellationToken ct = default);

      
        Task<List<Chamado>> ListarDoClienteAsync(Guid clienteId,int page,int pageSize, CancellationToken ct = default);
    }
}
