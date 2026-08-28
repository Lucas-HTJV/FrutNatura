using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.Core.Domain.DomainServices;

public static class SLAService
{
    /// <summary>
    /// Retorna o prazo-alvo (em horas) para primeira resposta conforme prioridade.
    /// </summary>
    public static TimeSpan PrazoPrimeiraResposta(Prioridade prioridade) => prioridade switch
    {
        Prioridade.Critica => TimeSpan.FromHours(1),
        Prioridade.Alta => TimeSpan.FromHours(4),
        Prioridade.Normal => TimeSpan.FromHours(8),
        Prioridade.Baixa => TimeSpan.FromHours(24),
        _ => TimeSpan.FromHours(8)
    };

    /// <summary>
    /// Exemplo de verificação simples se o prazo está estourado.
    /// </summary>
    public static bool EstourouPrazo(DateTime criadoEm, Prioridade prioridade, DateTime? agora = null)
    {
        agora ??= DateTime.UtcNow;
        return (agora.Value - criadoEm) > PrazoPrimeiraResposta(prioridade);
    }
}
