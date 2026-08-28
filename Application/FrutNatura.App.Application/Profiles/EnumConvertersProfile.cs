using AutoMapper;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.Profiles;

public sealed class EnumConvertersProfile : Profile
{
    public EnumConvertersProfile()
    {
        CreateMap<StatusChamado, string>().ConvertUsing(s => s.ToString());
        CreateMap<Prioridade, string>().ConvertUsing(p => p.ToString());
    }
}
