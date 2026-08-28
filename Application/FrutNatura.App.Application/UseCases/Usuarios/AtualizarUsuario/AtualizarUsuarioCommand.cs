using FrutNatura.App.Application.Common;

namespace FrutNatura.App.Application.UseCases.Usuarios.AtualizarUsuario;
public sealed record AtualizarUsuarioCommand(Guid Id, string Nome, bool? Ativo = null, IEnumerable<string>? Roles = null) : IRequest<Guid>, ITransactionalRequest;
