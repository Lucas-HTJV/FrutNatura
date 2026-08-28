using FrutNatura.Core.Domain.Enums;

public sealed class AbrirChamadoRequest
{
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public Prioridade? Prioridade { get; init; }  
}
