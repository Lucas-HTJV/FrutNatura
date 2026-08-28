namespace FrutNatura.Core.Abstractions.Common;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
