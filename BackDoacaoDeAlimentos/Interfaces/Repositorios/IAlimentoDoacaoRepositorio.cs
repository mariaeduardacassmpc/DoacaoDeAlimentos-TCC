using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IAlimentoDoacaoRepositorio
    {
        Task<AlimentoDoacao> Adicionar(AlimentoDoacao alimentoDoacao);
        Task<AlimentoDoacao> ObterPorId(int id);
        Task<IEnumerable<AlimentoDoacao>> ObterPorDoacao(int doacaoId);
        Task<IEnumerable<AlimentoDoacao>> ObterTodos();
        Task AtualizarAsync(AlimentoDoacao alimentoDoacao);
        Task RemoverAsync(int id);
    }
}
