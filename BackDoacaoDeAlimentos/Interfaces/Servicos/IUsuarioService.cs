using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterUsuarioPorId(int id);
        Task<Usuario> RegistrarUsuario(Usuario usuario);
        Task<Usuario> AutenticarUsuario(string email, string senha);
        //Task<Usuario> AutenticarUsuarioLogado(string email, string senha);
        Task AtualizarUsuario(Usuario usuario);
        Task RemoverUsuario(int id);
    }
}