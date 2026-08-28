using AutoMapper;
using FrutNatura.App.Application.UseCases.Chamados.AbrirChamado;
using FrutNatura.Core.Contracts;
using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.App.Application.Profiles;

public sealed class RequestsToCommandsProfile : Profile
{
    public RequestsToCommandsProfile()
    {
        CreateMap<AbrirChamadoRequest, AbrirChamadoCommand>();
        
    }
}
