namespace FrutNatura.Core.Contracts.Common;

/// <summary>
/// Modelo simples para erros padronizados em JSON (compatível com RFC7807).
/// </summary>
public sealed record ProblemDetailsExt(
    string Title,
    string Detail,
    int Status,
    string? Instance = null,
    string? Type = "about:blank");
