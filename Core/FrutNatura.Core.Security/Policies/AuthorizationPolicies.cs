using FrutNatura.Core.Security.Roles;

namespace FrutNatura.Core.Security.Policies;


public static class AuthorizationPolicies
{
    // Nomes de policies
    public const string ClienteOnly = "ClienteOnly";
    public const string StaffOnly = "StaffOnly";
    public const string SupervisorUp = "SupervisorUp";
    public const string AdminOnly = "AdminOnly";

    public static IReadOnlyDictionary<string, IReadOnlyList<string>> PolicyToRolesMap =>
        new Dictionary<string, IReadOnlyList<string>>
        {
            [ClienteOnly] = new[] { SystemRoles.Cliente },
            [StaffOnly] = new[] { SystemRoles.Atendente, SystemRoles.Supervisor, SystemRoles.Admin },
            [SupervisorUp] = new[] { SystemRoles.Supervisor, SystemRoles.Admin },
            [AdminOnly] = new[] { SystemRoles.Admin }
        };
}
