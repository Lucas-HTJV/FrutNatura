using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace FrutNatura.Infra.Repositories;

public sealed class RefreshTokensRepository : IRefreshTokensService
{
    private readonly FrutNaturaDbContext _ctx;
    public RefreshTokensRepository(FrutNaturaDbContext ctx) => _ctx = ctx;

    public Task AddAsync(RefreshToken token, CancellationToken ct = default)
        => _ctx.RefreshToken.AddAsync(token, ct).AsTask();

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        => _ctx.RefreshToken.AsNoTracking().FirstOrDefaultAsync(t => t.Token == token, ct);

    public async Task RevokeAllByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var tokens = await _ctx.RefreshToken
            .Where(t => t.UsuarioId == userId && t.RevokedUtc == null)
            .ToListAsync(ct);

        foreach (var t in tokens)
            t.RevokedUtc = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
    }
}
