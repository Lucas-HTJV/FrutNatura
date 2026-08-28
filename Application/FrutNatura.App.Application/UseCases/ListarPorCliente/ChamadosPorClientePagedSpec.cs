using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.ListarPorCliente;

public sealed class ChamadosPorClientePagedSpec : Specification<Chamado>
{
    public ChamadosPorClientePagedSpec(Guid clienteId, StatusChamado? status, Prioridade? prioridade, int page, int pageSize)
    {
        Query.Where(c => c.ClienteId == clienteId);

        if (status.HasValue)
            Query.Where(c => c.Status == status.Value);

        if (prioridade.HasValue)
            Query.Where(c => c.Prioridade == prioridade.Value);

        Query.OrderByDescending(c => c.CriadoEmUtc);

        var p = page < 1 ? 1 : page;
        var ps = pageSize <= 0 ? 20 : pageSize;

        Query.Skip((p - 1) * ps).Take(ps);
    }
}
