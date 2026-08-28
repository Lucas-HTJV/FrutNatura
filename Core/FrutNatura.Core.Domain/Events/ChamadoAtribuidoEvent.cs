namespace FrutNatura.Core.Domain.Events;

public sealed record ChamadoAtribuidoEvent(Guid ChamadoId, Guid ResponsavelId, DateTime Quando);
