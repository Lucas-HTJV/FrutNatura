using FrutNatura.App.Api.Contracts.Mensagens;
using FrutNatura.App.Application.UseCases.ListarPorCliente;
using FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;
using FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens;
using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Mensagens;
using FrutNatura.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrutNatura2.Controllers;

[ApiController]
[Route("api/chamados/{chamadoId:guid}/mensagens")]
public sealed class MensagensController : ControllerBase
{
    private readonly IMediator _mediator;
    public MensagensController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public Task<Guid> Enviar(Guid chamadoId, [FromBody] EnviarMensagemRequest body, CancellationToken ct)
    {
        var cmd = new EnviarMensagemCommand(
            chamadoId,
            body.AutorId,
            body.Conteudo?.Trim() ?? string.Empty
        
        );

        return _mediator.Send(cmd, ct);
    }

    [HttpGet]
    public Task<IReadOnlyList<MensagemDto>> ListarMensagens(
    Guid chamadoId,
    CancellationToken ct = default)
    {
        var q = new ListarMensagensQuery   
        {
            ChamadoId = chamadoId
        };

        return _mediator.Send(q, ct);
    }
}
