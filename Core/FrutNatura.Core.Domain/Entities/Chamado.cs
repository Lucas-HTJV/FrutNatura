using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.Core.Domain.Entities;

public class Chamado
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ClienteId { get; private set; }

    public string Titulo { get; private set; } = default!;
    public string Descricao { get; private set; } = default!;
    public StatusChamado Status { get; private set; } = StatusChamado.Aberto;
    public Prioridade Prioridade { get; private set; } = Prioridade.Normal;

    // NOVO: responsável (atendente) e data de fechamento
    public Guid? ResponsavelId { get; private set; }
    public DateTime CriadoEmUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? FechadoEmUtc { get; private set; }

    // EF
    private Chamado() { }

    private Chamado(Guid clienteId, string titulo, string descricao, StatusChamado? status = null)
    {
        if (clienteId == Guid.Empty) throw new ArgumentException("ClienteId inválido.", nameof(clienteId));

        titulo = (titulo ?? string.Empty).Trim();
        if (titulo.Length == 0) throw new ArgumentException("Título é obrigatório.", nameof(titulo));
        if (titulo.Length > 120) throw new ArgumentException("Título > 120.", nameof(titulo));

        descricao = (descricao ?? string.Empty).Trim();
        if (descricao.Length == 0) throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));
        if (descricao.Length > 4000) throw new ArgumentException("Descrição > 4000.", nameof(descricao));

        ClienteId = clienteId;
        Titulo = titulo;
        Descricao = descricao;

        if (status.HasValue) Status = status.Value;
    }
    public Chamado(Guid clienteId, string descricao)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));

        // se quiser, pode usar a descrição como título ou gerar um título padrão
        Titulo = descricao;

        Status = StatusChamado.Aberto;
        Prioridade = Prioridade.Normal;
        CriadoEmUtc = DateTime.UtcNow;
    }

    public static Chamado Abrir(Guid clienteId, string titulo, string descricao, Prioridade prioridade = Prioridade.Normal)
        => new Chamado(clienteId, titulo, descricao).WithPrioridade(prioridade);

    private Chamado WithPrioridade(Prioridade p)
    {
        Prioridade = p;
        return this;
    }

    // ===== Métodos usados pelos handlers =====

    
    public void Desatribuir() => ResponsavelId = null;

    public void AtribuirResponsavel(Guid responsavelId)
    {
        if (responsavelId == Guid.Empty)
            throw new ArgumentException("Responsável inválido.", nameof(responsavelId));

        ResponsavelId = responsavelId;
    }

    public void AlterarStatus(StatusChamado novo)
    {
        Status = novo;
        if (novo == StatusChamado.Fechado && FechadoEmUtc is null)
            FechadoEmUtc = DateTime.UtcNow;
        if (novo != StatusChamado.Fechado)
            FechadoEmUtc = null;
    }


}
