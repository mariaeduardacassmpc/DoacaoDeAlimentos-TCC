using System.Collections.Generic;
using System.Threading.Tasks;
using FrontDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface INotificacaoRepositorio
    {
        Task<Notificacao> AdicionarNotificacao(Notificacao notificacao);
        Task<Notificacao> ObterNotificacaoPorId(int id);
        Task<IEnumerable<Notificacao>> ObterTodasNotificacoes();
        Task DeletarNotificacao(int id);
    }
}