namespace FrutNatura.Core.Domain.Entities;

public class Mensagem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ChamadoId { get; private set; }
    public Guid? AutorId { get; private set; }
    public string Conteudo { get; private set; } = default!;
    public DateTime CriadoEmUtc { get; private set; } = DateTime.UtcNow;

    // EF
    private Mensagem() { }

    // Construtores privados para a factory
    private Mensagem(Guid chamadoId, string conteudo)
    {
        if (chamadoId == Guid.Empty) throw new ArgumentException("ChamadoId inválido.", nameof(chamadoId));
        conteudo = (conteudo ?? string.Empty).Trim();
        if (conteudo.Length == 0) throw new ArgumentException("Texto é obrigatório.", nameof(conteudo));
        if (conteudo.Length > 4000) throw new ArgumentException("Texto > 4000.", nameof(conteudo));

        ChamadoId = chamadoId;
        Conteudo = conteudo;
    }

    private Mensagem(Guid chamadoId, string texto, Guid? autorId) : this(chamadoId, texto)
    {
        AutorId = autorId;
    }

    // ✅ FÁBRICA ÚNICA USADA PELA APLICAÇÃO
    public static Mensagem Criar(Guid chamadoId, string texto, Guid? autorId = null)
        => autorId.HasValue ? new Mensagem(chamadoId, texto, autorId) : new Mensagem(chamadoId, texto);
}
