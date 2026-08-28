using Ardalis.Specification;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.Core.Abstractions.Specifications;


public sealed class ChamadosAbertosSpec : Specification<Chamado>
{
    public ChamadosAbertosSpec(StatusChamado status = StatusChamado.Aberto)
    {
        Query.Where(c => c.Status == status)
             .OrderByDescending(c => c.CriadoEmUtc);
    }
}
