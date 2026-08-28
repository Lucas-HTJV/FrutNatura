using System.Text.RegularExpressions;

namespace FrutNatura.Core.Domain.ValueObjects;

public readonly struct Email : IEquatable<Email>
{
    private static readonly Regex Rx = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public string Valor { get; }

    private Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) throw new ArgumentException("E-mail é obrigatório.");
        valor = valor.Trim();
        if (!Rx.IsMatch(valor)) throw new ArgumentException("E-mail inválido.");
        Valor = valor;
    }

    public static Email From(string valor) => new(valor);

    public override string ToString() => Valor;

    public bool Equals(Email other) => string.Equals(Valor, other.Valor, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is Email e && Equals(e);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Valor);

    public static bool operator ==(Email a, Email b) => a.Equals(b);
    public static bool operator !=(Email a, Email b) => !a.Equals(b);
}
