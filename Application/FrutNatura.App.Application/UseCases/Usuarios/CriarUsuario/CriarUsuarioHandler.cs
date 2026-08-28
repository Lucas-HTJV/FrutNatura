using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Security;
using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.App.Application.UseCases.Usuarios.CriarUsuario;

public sealed class CriarUsuarioHandler
{
    private readonly IUsuariosRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    public CriarUsuarioHandler(IUsuariosRepository repo, IUnitOfWork uow, IPasswordHasher hasher) { _repo = repo; _uow = uow; _hasher = hasher;}

    public async Task<Guid> Handle(CriarUsuarioCommand cmd, CancellationToken ct)
    {
        var hash = _hasher.HashPassword(cmd.HashPassword);
        var u = Usuario.Criar(cmd.Nome, cmd.Email, hash, cmd.Roles);
        await _repo.AddAsync(u, ct);
        
      
        return u.Id;
    }
}
