using AutoMapper;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.App.Application.Common
{
    public sealed class ApplicationProfiles : Profile
    {
        public ApplicationProfiles()
        {

            CreateMap<Chamado, ChamadoListDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Prioridade, opt => opt.MapFrom(s => s.Prioridade.ToString()));

        }
    }
}
