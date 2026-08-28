using AutoMapper;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Contracts.Mensagens;
using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.App.Application.Profiles;

public sealed class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Chamado, ChamadoDto>();
        CreateMap<Mensagem, MensagemDto>()
     .ForMember(d => d.Texto, opt => opt.MapFrom(s => s.Conteudo));

    }
}
