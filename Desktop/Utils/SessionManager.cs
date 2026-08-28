namespace FrutNatura.Desktop.Utils
{
    public static class SessionManager
    {
        public static string? AccessToken { get; set; }
        public static string? RefreshToken { get; set; }
        public static string? UserName { get; set; }
        public static string? UserRole { get; set; }
        public static Guid UsuarioId { get; set; } 
    }
}
