using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.Common;

public static class EnumMapper
{
    public static string ToText(StatusChamado s) => s switch
    {
        StatusChamado.Aberto => "Aberto",
        StatusChamado.EmAtendimento => "EmAtendimento",
        StatusChamado.AguardandoCliente => "AguardandoCliente",
        StatusChamado.Resolvido => "Resolvido",
        StatusChamado.Fechado => "Fechado",
        _ => "Aberto"
    };

    public static StatusChamado ParseStatus(string text)
    {
        text = (text ?? "").Trim();
        return Enum.TryParse<StatusChamado>(text, ignoreCase: true, out var val)
            ? val
            : StatusChamado.Aberto;
    }



    public static string ToText(Prioridade p) => p switch
    {
        Prioridade.Baixa => "Baixa",
        Prioridade.Normal => "Normal",
        Prioridade.Alta => "Alta",
        Prioridade.Critica => "Crítica",
        _ => "Normal"
    };

    public static Prioridade ParsePrioridade(string? text)
    {
        text = (text ?? "").Trim();

        return text.ToLower() switch
        {
            "baixa" => Prioridade.Baixa,
            "normal" => Prioridade.Normal,
            "alta" => Prioridade.Alta,
            "crítica" => Prioridade.Critica,
            "critica" => Prioridade.Critica,
            _ => Prioridade.Normal
        };
    }
}


