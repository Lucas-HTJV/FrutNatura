using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Infra.AI;

public sealed class EchoAiProvider : IAIProvider
{
    public Task<string> CompleteAsync(string prompt, CancellationToken ct = default)
        => Task.FromResult($"[echo] {prompt}");
}

