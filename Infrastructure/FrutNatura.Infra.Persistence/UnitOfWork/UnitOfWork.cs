using System.Threading;
using System.Threading.Tasks;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Infra.Persistence.Db;

namespace FrutNatura.Infra.Persistence.UniWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly FrutNaturaDbContext _context;

        public UnitOfWork(FrutNaturaDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
