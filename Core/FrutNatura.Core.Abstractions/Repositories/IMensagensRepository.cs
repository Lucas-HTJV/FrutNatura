using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Abstractions.Repositories;

public interface IMensagensRepository
{
    Task AddAsync(Mensagem entity, CancellationToken ct = default);
    Task<IReadOnlyList<Mensagem>> ListByChamadoAsync(Guid chamadoId, CancellationToken ct = default);

    // Alternativa com Specification (se preferir):
    Task<IReadOnlyList<Mensagem>> ListAsync(ISpecification<Mensagem> spec, CancellationToken ct = default);
}
