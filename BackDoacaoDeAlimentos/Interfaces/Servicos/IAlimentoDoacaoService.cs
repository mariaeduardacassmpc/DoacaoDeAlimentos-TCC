using TCCDoacaoDeAlimentos.Shared.Models;

public interface IAlimentoDoacaoService
{
    Task<AlimentoDoacao> AdicionarAlimentoADoacao(AlimentoDoacao alimentoDoacao);
    Task Remover(int id);
    Task<IEnumerable<AlimentoDoacao>> ListarAlimentosPorDoacao(int doacaoId);
    Task ValidarAlimentoDoacaoAsync(AlimentoDoacao alimentoDoacao);
    Task AtualizarQuantidadeAlimento(int id, decimal novaQuantidade);
    Task<IEnumerable<AlimentoDoacao>> ListarAlimentosProximosVencimento(int diasAntecedencia);
    Task<AlimentoDoacao> ObterPorId(int id);
}
