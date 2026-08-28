using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Chamados;           
using MediatR;

namespace FrutNatura.App.Application.UseCases.Chamados.ListarPorStaff;

public sealed class ListarChamadosStaffQuery : IRequest<PagedResult<ChamadoDto>>
{
    public string? Status { get; init; }           
           
    public Guid? ResponsavelId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
