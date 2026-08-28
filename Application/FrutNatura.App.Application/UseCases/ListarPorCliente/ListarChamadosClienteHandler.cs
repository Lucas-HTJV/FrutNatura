using AutoMapper;
using FrutNatura.Core.Abstractions.Common.PageResults;     
using FrutNatura.Core.Abstractions.Repositories;          
using FrutNatura.Core.Contracts.Chamados;                  
using MediatR;

namespace FrutNatura.App.Application.UseCases.ListarPorCliente;

public sealed class ListarChamadosClienteHandler
    : IRequestHandler<ListarChamadosClienteQuery, PagedResult<ChamadoListDto>>
{
    private readonly IChamadosRepository _repo;
    private readonly IMapper _mapper;

    public ListarChamadosClienteHandler(IChamadosRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<ChamadoListDto>> Handle(
        ListarChamadosClienteQuery request, CancellationToken ct)
    {
        var listSpec = new ChamadosPorClientePagedSpec(
            request.ClienteId, request.Status, request.Prioridade, request.Page, request.PageSize);

        var countSpec = new ChamadosPorClienteCountSpec(
            request.ClienteId, request.Status, request.Prioridade);

        var entities = await _repo.ListAsync(listSpec, ct);
        var total = await _repo.CountAsync(countSpec, ct);

        
        var items = _mapper.Map<IReadOnlyList<ChamadoListDto>>(entities);
        
        return new PagedResult<ChamadoListDto>(items, total, request.Page, request.PageSize);
    }
}
