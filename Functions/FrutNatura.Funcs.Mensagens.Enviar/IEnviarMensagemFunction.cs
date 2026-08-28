using FrutNatura.App.Api.Contracts.Mensagens;
using FrutNatura.Core.Contracts.Mensagens;

namespace FrutNatura.Funcs.Mensagens.Enviar
{
    public interface IEnviarMensagemFunction
    {
        Task ExecuteAsync(EnviarMensagemRequest request, CancellationToken ct = default);
    }
}
