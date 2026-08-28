using FrutNatura.App.Application.UseCases.Chamados.AlterarStatus;
using FrutNatura.App.Application.UseCases.Chamados.AtribuirChamado;
using FrutNatura.App.Application.UseCases.Chamados.ListarPorStaff;
using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Chamados;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrutNatura2.Controllers
{
    // Modelo para atribuição de responsável
    public sealed class AtribuirResponsavelRequest
    {
        public Guid ResponsavelId { get; set; }
    }

    [ApiController]
    [Route("api/staff/chamados")]
    public sealed class ChamadosStaffController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Construtor com injeção de dependência
        public ChamadosStaffController(IMediator mediator) => _mediator = mediator;

        // Endpoint para listar chamados com filtragem e paginação
        [HttpGet]
        public Task<PagedResult<ChamadoDto>> Listar(
            [FromQuery] string? status = null,
            [FromQuery] Guid? responsavelId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            return _mediator.Send(new ListarChamadosStaffQuery
            {
                Status = status,
                ResponsavelId = responsavelId,
                Page = page,
                PageSize = pageSize
            }, ct);
        }

        // Endpoint para alterar o status do chamado
        [HttpPost("{chamadoId:guid}/status")]
        public async Task<IActionResult> AlterarStatus(
            [FromRoute] Guid chamadoId,
            [FromBody] StatusChamado novoStatus,
            CancellationToken ct)
        {
            await _mediator.Send(new AlterarStatusCommand(chamadoId, novoStatus), ct);
            return NoContent();  // Retorna 204 (sem conteúdo) indicando sucesso
        }

        
       

        [HttpPut("{chamadoId:guid}/atribuir")]
        public async Task<IActionResult> AtribuirChamado(Guid chamadoId, [FromBody] AtribuirChamadoRequest request, CancellationToken ct)
        {
            var command = new AtribuirChamadoCommand
            {
                ChamadoId = chamadoId,
                ResponsavelId = request.ResponsavelId
            };

            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
