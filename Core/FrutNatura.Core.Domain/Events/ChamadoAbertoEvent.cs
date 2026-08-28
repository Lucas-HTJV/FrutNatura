namespace FrutNatura.Core.Domain.Events;

public sealed record ChamadoAbertoEvent(Guid ChamadoId, Guid ClienteId, string Protocolo, DateTime CriadoEm);
