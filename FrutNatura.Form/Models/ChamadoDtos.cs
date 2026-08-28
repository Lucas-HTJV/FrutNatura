using System;

namespace FrutNatura.Form.Models
{
    public sealed class ChamadoDtos
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public StatusChamado Status { get; set; }
        public Prioridade Prioridade { get; set; }
        public Guid? ResponsavelId { get; set; }
        public DateTime CriadoEmUtc { get; set; }
        public DateTime? FechadoEmUtc { get; set; }
    }
}
