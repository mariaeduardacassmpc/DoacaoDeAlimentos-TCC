using TCCDoacaoDeAlimentos.Shared.Models;
using TCCDoacaoDeAlimentos.Shared.Dto;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IEntidadeService
    {
        Task<IEnumerable<Entidade>> ObterOngsPorCidade(string cidade);
        Task<IEnumerable<Entidade>> ObterTodasOngs();
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task<Entidade> ObterEntidadePorId(int id);
        Task AdicionarEntidade(Entidade entidade, Usuario usuario);
        Task AtualizarEntidade(EntidadeEdicaoDto entidade);
        Task DeletarEntidade(int id);
        Task<bool> VerificarDocumentoeEmailExistente(string documento, string email);
    }
}
