using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura.Funcs.Auth.Login
{
    public interface ILoginFunction
    {
        Task<LoginResponse> ExecuteAsync(LoginRequest request, CancellationToken ct = default);
    }
}
