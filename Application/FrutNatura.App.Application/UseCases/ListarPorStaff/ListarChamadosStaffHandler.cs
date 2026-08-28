using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Contracts.Chamados;                       
using FrutNatura.Core.Domain.Enums;                   
using MediatR;

namespace FrutNatura.App.Application.UseCases.Chamados.ListarPorStaff;

public sealed class ListarChamadosStaffHandler
    : IRequestHandler<ListarChamadosStaffQuery, PagedResult<ChamadoDto>>
{
    private readonly IChamadosRepository _repo;

    public ListarChamadosStaffHandler(IChamadosRepository repo) => _repo = repo;

    public async Task<PagedResult<ChamadoDto>> Handle(ListarChamadosStaffQuery q, CancellationToken ct)
    {
        // parse string -> enum
        StatusChamado? statusEnum = null;
        if (!string.IsNullOrWhiteSpace(q.Status) &&
            Enum.TryParse<StatusChamado>(q.Status, true, out var parsed))
        {
            statusEnum = parsed;
        }

        var pagedSpec = new ChamadosStaffPagedSpec(statusEnum, q.ResponsavelId, q.Page, q.PageSize);
        var countSpec = new ChamadosStaffCountSpec(statusEnum, q.ResponsavelId);

        // seu repo retorna entidades (não projection)
        var entities = await _repo.ListAsync(pagedSpec, ct);
        var total = await _repo.CountAsync(countSpec, ct);

        // map manual p/ ChamadoDto (campos conforme seu screenshot)
        var items = entities.Select(c => new ChamadoDto
        {
            Id = c.Id,
            ClienteId = c.ClienteId,
            Titulo = c.Titulo,
            Descricao = c.Descricao,
            Status = c.Status,        // enum -> enum
            Prioridade = c.Prioridade,    // enum -> enum
            CriadoEmUtc = c.CriadoEmUtc,
            FechadoEmUtc = c.FechadoEmUtc,
            ResponsavelId = c.ResponsavelId
        }).ToList();

        return new PagedResult<ChamadoDto>(items, total, q.Page, q.PageSize);
    }
}
