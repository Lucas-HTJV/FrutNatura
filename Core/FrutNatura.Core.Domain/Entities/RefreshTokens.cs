namespace FrutNatura.Core.Domain.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresUtc { get; set; }
    public DateTime? RevokedUtc { get; set; }

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}
