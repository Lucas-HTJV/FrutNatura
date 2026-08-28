using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Contracts.Chamados; 
using FrutNatura.Core.Domain.Enums;      
using MediatR;

namespace FrutNatura.App.Application.UseCases.ListarPorCliente;

public sealed class ListarChamadosClienteQuery : IRequest<PagedResult<ChamadoListDto>>
{
    public Guid ClienteId { get; init; }

   
    public StatusChamado? Status { get; init; }
    public Prioridade? Prioridade { get; init; }

    
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
