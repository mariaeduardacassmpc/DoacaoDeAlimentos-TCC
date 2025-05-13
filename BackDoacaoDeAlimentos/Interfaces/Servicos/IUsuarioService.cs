using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterUsuarioPorIdAsync(int id);
        Task<Usuario> RegistrarUsuarioAsync(Usuario usuario);
        Task<Usuario> AutenticarUsuarioAsync(string email, string senha);
        Task AtualizarUsuarioAsync(Usuario usuario);
        Task RemoverUsuarioAsync(int id);
    }
}