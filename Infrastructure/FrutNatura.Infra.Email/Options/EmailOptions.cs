namespace FrutNatura.Infra.Email.Options;

public sealed class EmailOptions
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FromAddress { get; set; } = default!;
    public string FromName { get; set; } = "FrutNatura";
}
