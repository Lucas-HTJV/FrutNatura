using FrutNatura.Core.Abstractions.Services;

namespace FrutNatura.Funcs.Chamados.Atribuir;

public sealed class AtribuirChamadoFunction
{
    private readonly IChamadosService _chamados;
    public AtribuirChamadoFunction(IChamadosService chamados) => _chamados = chamados;

    public Task HandleAsync(Guid chamadoId, Guid? responsavelId, CancellationToken ct = default)
        => _chamados.AtribuirAsync(chamadoId, responsavelId, ct);
}
