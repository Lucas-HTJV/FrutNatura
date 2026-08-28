using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura.Funcs.Auth.Register;

public interface IRegisterFunction
{
    Task<AuthResult> HandleAsync(RegisterRequest request, CancellationToken ct = default);
}
