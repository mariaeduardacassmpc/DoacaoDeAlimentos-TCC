using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Task<Entidade> ObterPorEntidadeId(int id);
        Task<Entidade> ObterPorId(int id); 
        Task<Usuario> ObterPorEmail(string email);
        Task<Usuario> Adicionar(Usuario usuario);
        Task Atualizar(Usuario usuario);
        Task<bool> VerificarEmailExistente(string email);
        Task AtualizarSenha(Usuario usuario);
    }
}