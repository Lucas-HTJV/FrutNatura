using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.ValueObjects;
using FrutNatura.Infra.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrutNatura.Infra.Persistence;

public sealed class UsuariosRepository : IUsuariosRepository
{
    private readonly FrutNaturaDbContext _ctx;
    

    public UsuariosRepository(FrutNaturaDbContext ctx) => _ctx = ctx;


    public async Task AddAsync(Usuario entity, CancellationToken ct = default)
        => await _ctx.Set<Usuario>().AddAsync(entity, ct);

    public async Task<Usuario?> GetAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Set<Usuario>().FirstOrDefaultAsync(u => u.Id == id, ct);


    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = email.Trim().ToLowerInvariant();
        var emailVO = Email.From(normalized);

        return await _ctx.Set<Usuario>()
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Email == emailVO, ct);

    }
    public async Task<List<Usuario>> ObterUsuariosPorRoleAsync(string role)
    {
        return await _ctx.Usuarios
            .Where(u => u.Roles.Contains(role))  
            .ToListAsync();

    }
    public async Task<Usuario?> ObterPorIdAsync(Guid usuarioId)
    {
        return await _ctx.Usuarios.FindAsync(usuarioId);
    }
    public async Task AtribuirResponsavelAoChamadoAsync(Guid chamadoId, Guid responsavelId)
    {
        var chamado = await _ctx.Chamados.FindAsync(chamadoId);

        if (chamado is null)
            return; 

        chamado.AtribuirResponsavel(responsavelId);

        await _ctx.SaveChangesAsync();
    }




    public async Task<Usuario?> GetByIdAsync(Guid usuarioId, CancellationToken ct = default)
    {
        return await _ctx.Usuarios.FindAsync(new object[] { usuarioId }, ct);
    }

    public async Task Save(Usuario usuario, CancellationToken ct = default)
    {
        _ctx.Usuarios.Update(usuario);
        await _ctx.SaveChangesAsync(ct);  
    }

    public Task UpdateAsync(Usuario entity, CancellationToken ct = default)
    {
        _ctx.Set<Usuario>().Update(entity);
        return Task.CompletedTask;
    }
}

