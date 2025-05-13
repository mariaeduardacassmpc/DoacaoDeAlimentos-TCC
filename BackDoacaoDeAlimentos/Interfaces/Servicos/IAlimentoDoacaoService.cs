using TCCDoacaoDeAlimentos.Shared.Models;

public interface IAlimentoDoacaoService
{
    Task<AlimentoDoacao> AdicionarAlimentoADoacaoAsync(AlimentoDoacao alimentoDoacao);
    Task RemoverAlimentoDeDoacaoAsync(int id);
    Task<IEnumerable<AlimentoDoacao>> ListarAlimentosPorDoacaoAsync(int doacaoId);
    Task ValidarAlimentoDoacaoAsync(AlimentoDoacao alimentoDoacao);
    Task AtualizarQuantidadeAlimentoAsync(int id, decimal novaQuantidade);
    Task<IEnumerable<AlimentoDoacao>> ListarAlimentosProximosVencimentoAsync(int diasAntecedencia);
    Task<AlimentoDoacao> ObterPorIdAsync(int id);
}
