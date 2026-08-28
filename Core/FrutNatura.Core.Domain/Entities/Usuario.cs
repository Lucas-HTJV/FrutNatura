using System.ComponentModel.DataAnnotations.Schema;
using FrutNatura.Core.Domain.ValueObjects;

namespace FrutNatura.Core.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public bool Ativo { get; private set; } = true;
    public string PasswordHash { get; private set; } = string.Empty;

    // ---- Persistido no banco (coluna nvarchar) ----
    public string RolesSerialized { get; private set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get;} = new List<RefreshToken>();

    // ---- Conveniência em memória ----
    [NotMapped]
    public IReadOnlyCollection<string> Roles => _roles;
    private readonly List<string> _roles = new();

    private Usuario() { } // EF

    public static Usuario Criar(string nome, string email, string passwordHash, IEnumerable<string>? roles = null)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("E-mail é obrigatório.", nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash é obrigatório.", nameof(PasswordHash));

        var u = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Email = Email.From(email),
            PasswordHash = passwordHash,
            Ativo = true
        };

        if (roles is not null)
        {
            u._roles.AddRange(roles
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim()));
        }

        u.SyncRoles();
        return u;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;

    public void ConcederRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role)) return; 
        if (!_roles.Contains(role, StringComparer.OrdinalIgnoreCase)) 
        {
            _roles.Add(role); 
            SyncRoles();
        }
    }

    public void RevogarRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role)) return; 
        _roles.RemoveAll(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase)); 
        SyncRoles(); 
    }


    // Atualiza a string persistida
    private void SyncRoles() =>
        RolesSerialized = _roles.Count == 0
            ? string.Empty
            : string.Join(';', _roles);
}