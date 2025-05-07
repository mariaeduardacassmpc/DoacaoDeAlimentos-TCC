using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IEntidadeService
    {
        Task AdicionarEntidade(Entidade entidade);
        Task<Entidade> ObterEntidadePorId(int id);
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task AtualizarEntidade(Entidade entidade);
        Task DeletarEntidade(int id);
    }
}
