using AutoMapper;
using FrutNatura.App.Application.UseCases.Chamados.AbrirChamado;
using FrutNatura.App.Application.UseCases.Chamados.AtribuirChamado;
using FrutNatura.App.Application.UseCases.Chamados.ObterPorId;
using FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;
using FrutNatura.App.Application.UseCases.ListarPorCliente;
using FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens;
using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Chamados;
using FrutNatura.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;


namespace FrutNatura2.Controllers;

[ApiController]
[Route("api/clientes/chamados")]
public sealed class ChamadosClienteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChamadosClienteController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public Task<Guid> Abrir([FromHeader(Name = "Authorization")] string authorization, [FromBody] AbrirChamadoRequest body, CancellationToken ct)
    {
        var clienteId = GetClienteIdFromToken(authorization); 

        if (clienteId == Guid.Empty)
        {
            return Task.FromException<Guid>(new UnauthorizedAccessException("Cliente não autenticado."));
        }

        var cmd = new AbrirChamadoCommand
        {
            ClienteId = clienteId,
            Titulo = body?.Titulo?.Trim() ?? string.Empty,
            Descricao = body?.Descricao?.Trim() ?? string.Empty,
            Prioridade = body?.Prioridade ?? Prioridade.Normal
        };

        return _mediator.Send(cmd, ct);
    }

  

    private Guid GetClienteIdFromToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Guid.Empty;

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token.Replace("Bearer ", ""));
        var clienteIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub");

        if (clienteIdClaim == null || !Guid.TryParse(clienteIdClaim.Value, out var clienteId))
            return Guid.Empty;

        return clienteId;
    }


    [HttpGet]
    public Task<PagedResult<ChamadoListDto>> Listar([FromHeader(Name = "Authorization")] string authorization,
    int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var clienteId = GetClienteIdFromToken(authorization); 

        if (clienteId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("Cliente não autenticado.");
        }

        var q = new ListarChamadosClienteQuery
        {
            ClienteId = clienteId,
            Page = page,
            PageSize = pageSize
        };

        return _mediator.Send(q, ct);
    }

   

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChamadoDetalheDto>> Obter(
    Guid id,
    [FromHeader(Name = "Authorization")] string authorization,
    CancellationToken ct = default)
    {
        // 1) Pega o cliente do token (já existe esse método)
        var clienteId = GetClienteIdFromToken(authorization);
        if (clienteId == Guid.Empty)
            return Unauthorized("Cliente não autenticado.");

        // 2) Busca o chamado
        var chamado = await _mediator.Send(new ObterChamadoPorIdQuery(id), ct);
        if (chamado is null || chamado.ClienteId != clienteId)
            return NotFound(); // não existe ou não pertence a esse cliente

        // 3) Busca as mensagens do chamado
        var mensagens = await _mediator.Send(new ListarMensagensQuery
        {
            ChamadoId = id
        }, ct);

        // 4) Monta o DTO de detalhes
        var dto = new ChamadoDetalheDto
        {
            Chamado = chamado,
            Mensagens = mensagens.ToList()
        };

        return Ok(dto);
    }

    [HttpPost("{id:guid}/mensagens")]
    public async Task<IActionResult> EnviarMensagem(
    Guid id,
    [FromHeader(Name = "Authorization")] string authorization,
    [FromBody] EnviarMensagemClienteRequest body,
    CancellationToken ct = default)
    {
        var clienteId = GetClienteIdFromToken(authorization);
        if (clienteId == Guid.Empty)
            return Unauthorized("Cliente não autenticado.");

        if (string.IsNullOrWhiteSpace(body.Texto))
            return BadRequest("Mensagem vazia.");

        // verifica se o chamado pertence ao cliente
        var chamado = await _mediator.Send(new ObterChamadoPorIdQuery(id), ct);
        if (chamado == null || chamado.ClienteId != clienteId)
            return NotFound();

        // envia mensagem
        await _mediator.Send(
        new EnviarMensagemCommand(
             chamadoId: id,
             autorId: clienteId,
             conteudo: body.Texto
            ),
            ct
        );


        return Ok();
    }


}

