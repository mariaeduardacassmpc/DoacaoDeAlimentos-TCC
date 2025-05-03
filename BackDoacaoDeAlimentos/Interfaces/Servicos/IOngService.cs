using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IOngService
    {
        Task AdicionarOng(Entidade ong);
        //Task<Ong> ObterOngPorId(int id);
        Task<IEnumerable<Entidade>> ObterTodasOngs();
        Task AtualizarOng(Entidade ong);
        Task DeletarOng(int id);
    }
}
