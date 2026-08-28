namespace FrutNatura.Core.Abstractions.Services;

public interface IMensagensService
{
  
    Task<Guid> EnviarAsync(Guid chamadoId, string texto, Guid? autorId, CancellationToken ct = default);
}
