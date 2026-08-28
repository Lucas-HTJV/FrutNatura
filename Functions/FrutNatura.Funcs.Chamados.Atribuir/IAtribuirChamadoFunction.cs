namespace FrutNatura.Funcs.Chamados.Atribuir
{
    public interface IAtribuirChamadoFunction
    {
        Task ExecuteAsync(int chamadoId, int agenteId, CancellationToken ct = default);
    }
}
