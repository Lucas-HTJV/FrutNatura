using FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens;
using FrutNatura.Core.Abstractions.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IAIProvider _ai;
    private readonly IMediator _mediator;

    public ChatController(IAIProvider ai, IMediator mediator)
    {
        _ai = ai;
        _mediator = mediator;
    }

    public record ChatRequest(Guid? ChamadoId, string Message);
    public record ChatResponse(string Reply);

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new ChatResponse("Mensagem vazia."));

        try
        {
            var contexto = new StringBuilder();

            // 👉 MODO 1: CHAT VINCULADO A UM CHAMADO (APP/WEB/ATENDENTE)
            if (request.ChamadoId.HasValue && request.ChamadoId.Value != Guid.Empty)
            {
                var mensagens = await _mediator.Send(
                    new ListarMensagensQuery { ChamadoId = request.ChamadoId.Value }, ct
                );

                contexto.AppendLine("Histórico do chamado:");
                foreach (var msg in mensagens)
                {
                    var autor = msg.AutorId == null ? "Cliente" : "Atendente";
                    contexto.AppendLine($"{autor}: {msg.Texto}");
                }

                contexto.AppendLine();
                contexto.AppendLine("Nova mensagem do cliente:");
                contexto.AppendLine(request.Message);
            }
            else
            {
                // 👉 MODO 2: CHAT GERAL NO SITE (NÃO TEM CHAMADO)
                contexto.AppendLine("Você é um atendente virtual do hortifrúti FrutNatura.");
                contexto.AppendLine("Atenda o cliente de forma educada e objetiva.");
                contexto.AppendLine("Você pode responder dúvidas sobre:");
                contexto.AppendLine("- produtos e preços;");
                contexto.AppendLine("- promoções;");
                contexto.AppendLine("- entregas;");
                contexto.AppendLine("- funcionamento da loja;");
                contexto.AppendLine("- disponibilidade de frutas e verduras;");
                contexto.AppendLine("- serviços.");
                contexto.AppendLine();
                contexto.AppendLine("Mensagem do cliente:");
                contexto.AppendLine(request.Message);
            }

            var resposta = await _ai.CompleteAsync(contexto.ToString(), ct);

            if (string.IsNullOrWhiteSpace(resposta))
                resposta = "Desculpe, não consegui gerar uma resposta no momento.";

            return Ok(new ChatResponse(resposta.Trim()));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatController] Erro: {ex.Message}");
            return StatusCode(500, new ChatResponse("Erro interno ao processar a solicitação."));
        }
    }
}
