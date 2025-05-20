using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IEntidadeRepositorio
    {
        Task AdicionarEntidade(Entidade entidade);
        Task<Entidade> ObterEntidadePorId(int id);
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task AtualizarEntidade(Entidade entidade);
        Task DeletarEntidade(int id);
        Task<IEnumerable<Entidade>> BuscarOngsPorCidade(string cidade);
        Task<bool> VerificarCnpjExistente(string documento);
    }
}
