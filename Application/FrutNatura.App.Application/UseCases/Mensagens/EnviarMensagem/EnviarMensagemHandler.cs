using AutoMapper;
using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Notifications;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using MediatR;

namespace FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;



public sealed class EnviarMensagemHandler : IRequestHandler<EnviarMensagemCommand, Guid>
{
    private readonly IChamadosRepository _chamados;
    private readonly IMensagensRepository _mensagens;
    private readonly IUnitOfWork _uow;
    private readonly IRealtimeNotifier _notifier;
    private readonly IMapper _mapper;

    public EnviarMensagemHandler(
        IChamadosRepository chamados,
        IMensagensRepository mensagens,
        IUnitOfWork uow,
        IRealtimeNotifier notifier,
        IMapper mapper)
    {
        _chamados = chamados;
        _mensagens = mensagens;
        _uow = uow;
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(EnviarMensagemCommand request, CancellationToken cancellationToken)
    {
        var chamado = await _chamados.GetByIdAsync(request.ChamadoId, cancellationToken);
        if (chamado is null)
            throw new InvalidOperationException("Chamado não encontrado.");

        // ✅ usa a fábrica do domínio (compatível com qualquer assinatura interna)
        var mensagem = Mensagem.Criar(request.ChamadoId, request.Conteudo, request.AutorId);

        await _mensagens.AddAsync(mensagem, cancellationToken);
   

        await _notifier.NotifyMensagemEnviadaAsync(mensagem.ChamadoId, mensagem.Id, cancellationToken);

        return mensagem.Id;
    }
}
