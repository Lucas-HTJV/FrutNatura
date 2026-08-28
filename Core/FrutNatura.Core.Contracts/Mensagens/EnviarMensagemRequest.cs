namespace FrutNatura.App.Api.Contracts.Mensagens;

public sealed class EnviarMensagemRequest
{
    public Guid AutorId { get; init; }
    public string Conteudo { get; init; } = string.Empty;
    public bool? VisivelParaCliente { get; init; }  
}
