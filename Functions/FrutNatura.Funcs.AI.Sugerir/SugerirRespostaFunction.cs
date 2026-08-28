using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Funcs.AI.Sugerir;

public sealed class SugerirRespostaFunction
{
    private readonly IAIProvider _ai;
    public SugerirRespostaFunction(IAIProvider ai) => _ai = ai;

    public Task<string> HandleAsync(string prompt, CancellationToken ct = default)
        => _ai.CompleteAsync(prompt, ct);
}
