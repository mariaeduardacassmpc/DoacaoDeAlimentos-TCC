using System.Collections.Generic;
using System.Threading.Tasks;
using FrontDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface INotificacaoService
    {
        Task EnviarEmailConfirmacaoDoacaoDoador(Doacao doacao);
        Task EnviarEmailConfirmacaoDoacaoOng(Doacao doacao);
        Task<bool> EnviarRecuperacaoSenha(string email);
    }
}