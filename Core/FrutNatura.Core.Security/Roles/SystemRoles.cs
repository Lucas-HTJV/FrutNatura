namespace FrutNatura.Core.Security.Roles;

/// <summary>
/// Papéis (roles) padrão do sistema.
/// </summary>
public static class SystemRoles
{
    public const string Cliente = "Cliente";
    public const string Atendente = "Atendente";
    public const string Supervisor = "Supervisor";
    public const string Admin = "Admin";

    /// <summary>Lista conveniente de todos os roles.</summary>
    public static readonly IReadOnlyList<string> All = new[]
    {
        Cliente, Atendente, Supervisor, Admin
    };
}
