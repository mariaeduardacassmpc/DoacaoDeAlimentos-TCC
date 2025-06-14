using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IAutenticacaoService
    {
        string GerarHashSenha(string senha);
        bool VerificarSenha(string senhaDigitada, string senhaHash);
        void Login(string login, string senha);
        (bool valido, string email) ValidarToken(string token);
        Task RedefinirSenha(string token, string senha);
    }
}
