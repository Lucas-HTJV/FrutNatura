using Ardalis.Specification;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.Chamados.ListarPorStaff;

public sealed class ChamadosStaffCountSpec : Specification<Chamado>
{
    
        public ChamadosStaffCountSpec(StatusChamado? status, Guid? responsavelId)
        {
            Query.AsNoTracking();

            if (status.HasValue)
                Query.Where(c => c.Status == status.Value);

            if (responsavelId.HasValue && responsavelId.Value != Guid.Empty)
                Query.Where(c => c.ResponsavelId == responsavelId); // Filtro de responsável direto
        }
    
}
