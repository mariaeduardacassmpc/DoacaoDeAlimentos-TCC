using System.Collections.Generic;
using System.Threading.Tasks;
using FrontDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface INotificacaoService
    {
        Task<IEnumerable<Notificacao>> ObterTodasNotificacoes();
        Task<Notificacao> ObterNotificacaoPorId(int id);
        Task<Notificacao> AdicionarNotificacao(Notificacao notificacao);
        Task RemoverNotificacao(int id);
    }
}