namespace FrutNatura.Core.Contracts.Chamados;

public sealed class ChamadoListDto
{
    public Guid Id { get; init; }
    public Guid ClienteId { get; init; }
    public string Titulo { get; init; } = default!;
    public string Status { get; init; } = default!;
    public DateTime CriadoEmUtc { get; init; }

    public string Prioridade { get; init; } = default!;
}
