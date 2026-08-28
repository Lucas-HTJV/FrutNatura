using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.Chamados.AbrirChamado;



public sealed class AbrirChamadoHandler : IRequestHandler<AbrirChamadoCommand, Guid>
{
    private readonly IChamadosRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IRealtimeNotifier _notifier;
    private readonly IMensagensRepository _mensagens;
    private readonly IMapper _mapper;

    public AbrirChamadoHandler(
        IChamadosRepository repo,
        IUnitOfWork uow,
        IMensagensRepository mensagens,
        IRealtimeNotifier notifier,
        IMapper mapper)
    {
        _repo = repo;
        _mensagens = mensagens;
        _uow = uow;
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AbrirChamadoCommand request, CancellationToken cancellationToken)
    {
        var chamado = Chamado.Abrir(request.ClienteId, request.Titulo, request.Descricao, request.Prioridade);

        await _repo.AddAsync(chamado, cancellationToken);

        var mensagemInicial = Mensagem.Criar(
        chamado.Id,
        request.Descricao,
        request.ClienteId);
        await _mensagens.AddAsync(mensagemInicial, cancellationToken);

        await _notifier.NotifyChamadoAbertoAsync(chamado.Id, chamado.ClienteId, cancellationToken);
        await _notifier.NotifyMensagemEnviadaAsync(chamado.Id, mensagemInicial.Id, cancellationToken);
        return chamado.Id;
    }
}
