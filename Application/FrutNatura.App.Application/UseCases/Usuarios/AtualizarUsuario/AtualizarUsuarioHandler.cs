using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions.Repositories;
using MediatR;
using System.Reflection;

namespace FrutNatura.App.Application.UseCases.Usuarios.AtualizarUsuario;

public sealed class AtualizarUsuarioHandler
    : IRequestHandler<AtualizarUsuarioCommand, Guid>
{
    private readonly IUsuariosRepository _repo;

    public AtualizarUsuarioHandler(IUsuariosRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(AtualizarUsuarioCommand request, CancellationToken ct)
    {
        var u = await _repo.GetAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Usuário não encontrado.");

        // Atualizar nome
        if (!string.IsNullOrWhiteSpace(request.Nome))
        {
            typeof(FrutNatura.Core.Domain.Entities.Usuario)
                .GetProperty("Nome")!
                .SetValue(u, request.Nome.Trim());
        }

        // Atualizar ativo
        if (request.Ativo.HasValue)
        {
            typeof(FrutNatura.Core.Domain.Entities.Usuario)
                .GetProperty("Ativo")!
                .SetValue(u, request.Ativo.Value);
        }

        // Atualizar roles
        if (request.Roles is not null)
        {
            var rolesField = typeof(FrutNatura.Core.Domain.Entities.Usuario)
                .GetField("_roles", BindingFlags.NonPublic | BindingFlags.Instance)!;

            rolesField.SetValue(u, request.Roles.ToList());
        }

        await _repo.UpdateAsync(u, ct);

        return u.Id;
    }
}
