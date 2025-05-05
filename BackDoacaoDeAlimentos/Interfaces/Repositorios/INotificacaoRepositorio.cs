using TCCDoacaoDeAlimentos.Shared.Models;
using FrontDoacaoDeAlimentos.Models; 
namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface INotificacaoRepositorio
    {
        Task<IEnumerable<Notificacao>> ObterTodasNotificacoes();
        Task<Notificacao> ObterNotificacaoPorId(int id);
        Task<Notificacao> AdicionarNotificacao(Notificacao notificacao);
    }
}
