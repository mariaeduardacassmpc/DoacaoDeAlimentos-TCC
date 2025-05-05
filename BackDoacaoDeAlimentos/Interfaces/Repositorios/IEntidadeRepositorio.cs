using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IEntidadeRepositorio
    {
        Task AdicionarOng(Entidade ong);
        Task<Entidade> ObterOngPorId(int id);
        Task<IEnumerable<Entidade>> ObterTodasOngs();
        Task AtualizarOng(Entidade ong);
        Task DeletarOng(int id);
    }
}
