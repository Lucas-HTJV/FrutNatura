using FrutNatura.Core.Domain.Entities; 
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FrutNatura.Core.Abstractions.Repositories
{
    public interface IRefreshTokensService
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task RevokeAllByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
