using System;

namespace FrutNatura.Form.Models
{
    public sealed class MensagemDto
    {
        public Guid Id { get; set; }
        public Guid ChamadoId { get; set; }
        public Guid? AutorId { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public DateTime CriadoEmUtc { get; set; }
    }
}
