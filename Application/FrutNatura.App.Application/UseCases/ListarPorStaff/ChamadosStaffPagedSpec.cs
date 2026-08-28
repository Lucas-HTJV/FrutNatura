using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.Chamados.ListarPorStaff;

public sealed class ChamadosStaffPagedSpec : Specification<Chamado>
{
    public ChamadosStaffPagedSpec(StatusChamado? status, Guid? responsavelId, int page, int pageSize)
    {
        Query.AsNoTracking();

        if (status.HasValue)
            Query.Where(c => c.Status == status.Value);

        if (responsavelId.HasValue && responsavelId.Value != Guid.Empty)
            Query.Where(c => c.ResponsavelId == responsavelId); // Filtro de responsável direto

        Query.OrderByDescending(c => c.CriadoEmUtc);

        var skip = Math.Max(0, (page - 1) * pageSize);
        Query.Skip(skip).Take(pageSize);
    }
}
