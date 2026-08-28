namespace FrutNatura.Funcs.AI.Sugerir
{
    public interface ISugerirRespostaFunction
    {
        Task<string> ExecuteAsync(int chamadoId, string prompt, CancellationToken ct = default);
    }
}
