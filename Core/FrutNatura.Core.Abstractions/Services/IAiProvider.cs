namespace FrutNatura.Core.Abstractions.Services;

public interface IAIProvider
{
    Task<string> CompleteAsync(string prompt, CancellationToken ct = default);
}

