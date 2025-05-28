using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IEntidadeRepositorio
    {
        Task <int> AdicionarEntidade(Entidade entidade);
        Task<Entidade> ObterEntidadePorId(int id);
        Task<IEnumerable<Entidade>> ObterTodasOngs();
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task AtualizarEntidade(Entidade entidade);
        Task DeletarEntidade(int id);
        Task<IEnumerable<Entidade>> BuscarOngsPorCidade(string cidade);
        Task<bool> VerificaDocumentoeEmailExistente(string documento, string email);
    }
}
