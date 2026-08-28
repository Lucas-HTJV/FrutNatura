using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace FrutNatura.Infra.Persistence;

public sealed class MensagensRepository : IMensagensRepository
{
    private readonly FrutNaturaDbContext _ctx;

    public MensagensRepository(FrutNaturaDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(Mensagem entity, CancellationToken ct = default)
        => await _ctx.Set<Mensagem>().AddAsync(entity, ct);

    public async Task<IReadOnlyList<Mensagem>> ListByChamadoAsync(Guid chamadoId, CancellationToken ct = default)
        => await _ctx.Set<Mensagem>()
                     .Where(m => m.ChamadoId == chamadoId)
                     .OrderBy(m => m.CriadoEmUtc)
                     .ToListAsync(ct);

    public async Task<IReadOnlyList<Mensagem>> ListAsync(ISpecification<Mensagem> spec, CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(_ctx.Set<Mensagem>().AsQueryable(), spec);
        return await query.ToListAsync(ct);
    }
}
