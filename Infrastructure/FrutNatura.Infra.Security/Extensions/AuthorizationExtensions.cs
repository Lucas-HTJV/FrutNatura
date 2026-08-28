using FrutNatura.Core.Security.Policies;
using FrutNatura.Core.Security.Roles;
using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Infra.Security.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.ClienteOnly,
                p => p.RequireRole(SystemRoles.Cliente));

            options.AddPolicy(AuthorizationPolicies.StaffOnly,
                p => p.RequireRole(SystemRoles.Atendente, SystemRoles.Supervisor, SystemRoles.Admin));

            options.AddPolicy(AuthorizationPolicies.SupervisorUp,
                p => p.RequireRole(SystemRoles.Supervisor, SystemRoles.Admin));

            options.AddPolicy(AuthorizationPolicies.AdminOnly,
                p => p.RequireRole(SystemRoles.Admin));
        });

        return services;
    }
}
