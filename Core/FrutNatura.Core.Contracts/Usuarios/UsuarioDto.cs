namespace FrutNatura.Core.Contracts.Usuarios;

public sealed record UsuarioDto(
    Guid Id,
    string Nome,
    string Email,
    bool Ativo,
    IReadOnlyList<string> Roles);
