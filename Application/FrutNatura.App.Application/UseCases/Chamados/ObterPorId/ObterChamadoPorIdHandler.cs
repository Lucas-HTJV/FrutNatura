using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.App.Application.UseCases.Chamados.ObterPorId;



public sealed class ObterChamadoPorIdHandler : IRequestHandler<ObterChamadoPorIdQuery, ChamadoDto?>
{
    private readonly IChamadosRepository _repo;
    private readonly IMapper _mapper;

    public ObterChamadoPorIdHandler(IChamadosRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ChamadoDto?> Handle(ObterChamadoPorIdQuery request, CancellationToken cancellationToken)
    {
        var chamado = await _repo.GetByIdAsync(request.Id, cancellationToken);
        return chamado is null ? null : _mapper.Map<ChamadoDto>(chamado);
    }
}
