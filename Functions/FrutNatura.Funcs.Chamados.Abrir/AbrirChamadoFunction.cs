using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Funcs.Chamados.Abrir;

public sealed class AbrirChamadoFunction
{
    private readonly IChamadosService _chamados;
    public AbrirChamadoFunction(IChamadosService chamados) => _chamados = chamados;

    public Task<Guid> HandleAsync(Guid clienteId, string titulo, string descricao, CancellationToken ct = default)
        => _chamados.AbrirAsync(clienteId, titulo, descricao, ct);
}
