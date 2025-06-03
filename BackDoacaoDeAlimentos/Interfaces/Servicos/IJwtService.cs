using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IJwtService
    {
        public string GerarToken(Usuario usuario);
    }
}
