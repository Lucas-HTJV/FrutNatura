namespace FrutNatura.Core.Contracts.Usuarios;

public sealed record CriarUsuarioRequest(
    string Nome,
    string Email,
    string Password,
    IReadOnlyList<string>? Roles = null);
