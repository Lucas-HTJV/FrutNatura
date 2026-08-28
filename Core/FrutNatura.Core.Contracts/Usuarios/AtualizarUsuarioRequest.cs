namespace FrutNatura.Core.Contracts.Usuarios;

public sealed record AtualizarUsuarioRequest(
    Guid Id,
    string Nome,
    bool? Ativo = null,
    IReadOnlyList<string>? Roles = null);
