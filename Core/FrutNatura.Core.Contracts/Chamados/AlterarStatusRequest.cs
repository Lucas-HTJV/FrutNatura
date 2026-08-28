namespace FrutNatura.Core.Contracts.Chamados;

/// <summary>
/// Ex.: "EmAtendimento", "AguardandoCliente", "Resolvido", "Fechado".
/// </summary>
public sealed record AlterarStatusRequest(Guid ChamadoId, string NovoStatus);
