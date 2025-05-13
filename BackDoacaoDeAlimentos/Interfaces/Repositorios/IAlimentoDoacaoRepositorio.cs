using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IAlimentoDoacaoRepositorio
    {
        Task<AlimentoDoacao> AdicionarAsync(AlimentoDoacao alimentoDoacao);
        Task<AlimentoDoacao> ObterPorIdAsync(int id);
        Task<IEnumerable<AlimentoDoacao>> ObterPorDoacaoAsync(int doacaoId);
        Task<IEnumerable<AlimentoDoacao>> ObterTodosAsync();
        Task AtualizarAsync(AlimentoDoacao alimentoDoacao);
        Task RemoverAsync(int id);
    }
}
