using FrutNatura.Core.Contracts.Mensagens;
using System.Collections.Generic;

namespace FrutNatura.Core.Contracts.Chamados;

public sealed class ChamadoDetalheDto
{
    public ChamadoDto Chamado { get; init; } = default!;
    public IReadOnlyList<MensagemDto> Mensagens { get; init; } = Array.Empty<MensagemDto>();
}
