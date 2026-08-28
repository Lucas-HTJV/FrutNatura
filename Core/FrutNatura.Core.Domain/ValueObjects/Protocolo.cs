using System.Text.RegularExpressions;

namespace FrutNatura.Core.Domain.ValueObjects;

public readonly struct Protocolo : IEquatable<Protocolo>
{
    private static readonly Regex Rx = new("^[A-Z0-9-]{6,32}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string Valor { get; }

    private Protocolo(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) throw new ArgumentException("Protocolo é obrigatório.");
        if (!Rx.IsMatch(valor)) throw new ArgumentException("Protocolo em formato inválido.");
        Valor = valor;
    }

    public static Protocolo Novo()
    {
        // simples: 6 primeiros do GUID + ano. Você pode injetar um serviço gerador se preferir.
        var head = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return new Protocolo($"{head}-{DateTime.UtcNow:yyyy}");
    }

    public static Protocolo From(string valor) => new(valor);

    public override string ToString() => Valor;

    public bool Equals(Protocolo other) => string.Equals(Valor, other.Valor, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is Protocolo p && Equals(p);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Valor);

    public static bool operator ==(Protocolo a, Protocolo b) => a.Equals(b);
    public static bool operator !=(Protocolo a, Protocolo b) => !a.Equals(b);
}
