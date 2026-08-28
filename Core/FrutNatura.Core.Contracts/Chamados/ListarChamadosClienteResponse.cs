namespace FrutNatura.Core.Contracts.Chamados;

public sealed record ListarChamadosClienteResponse(IReadOnlyList<ChamadoDto> Itens);
