using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Abstractions.Repositories;

public interface IUsuariosRepository
{
    Task AddAsync(Usuario usuario, CancellationToken ct = default);
    Task<Usuario?> GetAsync(Guid id, CancellationToken ct = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task UpdateAsync(Usuario usuario, CancellationToken ct = default);
    Task<List<Usuario>> ObterUsuariosPorRoleAsync(string role);  
    Task<Usuario?> ObterPorIdAsync(Guid usuarioId); 
    Task AtribuirResponsavelAoChamadoAsync(Guid chamadoId, Guid responsavelId);
    Task<Usuario?> GetByIdAsync(Guid usuarioId, CancellationToken ct = default);
    Task Save(Usuario usuario, CancellationToken ct = default);

}
