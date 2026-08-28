using FrutNatura.Infra.Persistence.Db;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FrutNatura.WebApi.Setup.Health;

public sealed class DbContextHealthCheck : IHealthCheck
{
    private readonly FrutNaturaDbContext _db;
    public DbContextHealthCheck(FrutNaturaDbContext db) => _db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            var ok = await _db.Database.CanConnectAsync(ct);
            return ok ? HealthCheckResult.Healthy("DB ok") : HealthCheckResult.Unhealthy("DB down");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("DB error", ex);
        }
    }
}
