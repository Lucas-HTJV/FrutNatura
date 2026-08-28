using FrutNatura.App.Application.Common;

namespace FrutNatura.App.Application.UseCases.Usuarios.CriarUsuario;
public sealed record CriarUsuarioCommand(string Nome, string Email, string HashPassword, IEnumerable<string>? Roles = null) : ITransactionalRequest;
