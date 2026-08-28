namespace FrutNatura.Core.Contracts.Mensagens;

public sealed class MensagemDto
{
    public Guid Id { get; init; }
    public Guid ChamadoId { get; init; }
    public Guid? AutorId { get; init; }
    public string Texto { get; init; } = default!;
    public DateTime CriadoEmUtc { get; init; }
}
