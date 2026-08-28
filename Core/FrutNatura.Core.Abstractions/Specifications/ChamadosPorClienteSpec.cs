using Ardalis.Specification;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.Core.Abstractions.Specifications;


public sealed class ChamadosPorClienteSpec : Specification<Chamado>
{
    public ChamadosPorClienteSpec(Guid clienteId, StatusChamado? status = null, Prioridade? prioridade = null)
    {
        Query.Where(c => c.ClienteId == clienteId);

        if (status.HasValue)
            Query.Where(c => c.Status == status.Value);

        if (prioridade.HasValue)
            Query.Where(c => c.Prioridade == prioridade.Value);

        Query.OrderByDescending(c => c.CriadoEmUtc);
    }
}
