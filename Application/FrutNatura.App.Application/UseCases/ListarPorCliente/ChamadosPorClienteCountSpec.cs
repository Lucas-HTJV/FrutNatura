using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.ListarPorCliente;

public sealed class ChamadosPorClienteCountSpec : Specification<Chamado>
{
    public ChamadosPorClienteCountSpec(Guid clienteId, StatusChamado? status, Prioridade? prioridade)
    {
        Query.Where(c => c.ClienteId == clienteId);

        if (status.HasValue)
            Query.Where(c => c.Status == status.Value);

        if (prioridade.HasValue)
            Query.Where(c => c.Prioridade == prioridade.Value);
    }
}
