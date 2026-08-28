using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Contracts.Mensagens;

namespace FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens;

public sealed class ListarMensagensHandler
    : IRequestHandler<ListarMensagensQuery, IReadOnlyList<MensagemDto>>
{
    private readonly IMensagensRepository _repo;
    private readonly IMapper _mapper;

    public ListarMensagensHandler(IMensagensRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MensagemDto>> Handle(
        ListarMensagensQuery request,
        CancellationToken cancellationToken)
    {
        var mensagens = await _repo.ListByChamadoAsync(request.ChamadoId, cancellationToken);

        

        return _mapper.Map<IReadOnlyList<MensagemDto>>(mensagens);
    }
}
