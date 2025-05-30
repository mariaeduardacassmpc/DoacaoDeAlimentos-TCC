using System.Transactions;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IEntidadeRepositorio
    {
        Task<IEnumerable<Entidade>> ObterOngsPorCidade(string cidade);
        Task<IEnumerable<Entidade>> ObterTodasOngs();
        Task<IEnumerable<Entidade>> ObterTodasEntidades();
        Task<Entidade> ObterEntidadePorId(int id);
        Task<int> AdicionarEntidade(Entidade entidade);
        Task AtualizarEntidade(EntidadeEdicaoDto entidade);
        Task InativarEntidade(int id);
        Task<bool> VerificaDocumentoeEmailExistente(string documento, string email);
    }
}
