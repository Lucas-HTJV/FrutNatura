using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Contracts.Chamados;

public sealed class ChamadoDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }

    public string Titulo { get; set; } = default!;
    public string Descricao { get; set; } = default!;

   
    public FrutNatura.Core.Domain.Enums.StatusChamado Status { get; set; }
    public FrutNatura.Core.Domain.Enums.Prioridade Prioridade { get; set; }

    public DateTime CriadoEmUtc { get; set; }
    public Guid? ResponsavelId { get; set; }
    public DateTime? FechadoEmUtc { get; set; }
}
