using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IJwtService
    {
        public string GerarToken(Usuario usuario, string cidade);
        public string GerarTokenRecuperacao(string email);
    }
}
