using TCCDoacaoDeAlimentos.Shared.Models;
using TCCDoacaoDeAlimentos.Shared.Dto;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IEntidadeService
    {
        Task<IEnumerable<Entidade>> ObterInstituicaoPorCidade(string cidade);
        Task<IEnumerable<Entidade>> ObterTodasInstituicoes();
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task<Entidade> ObterEntidadePorId(int id);
        Task AdicionarEntidade(Entidade entidade, Usuario usuario);
        Task AtualizarEntidade(EntidadeEdicaoDto entidade);
        Task InativarEntidade(int id);
        Task<bool> VerificarDocumentoeEmailExistente(string documento, string email);
    }
}
