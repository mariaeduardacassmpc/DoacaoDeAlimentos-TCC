using System.Threading.Tasks;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IGeocodingService
    {
        Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude);
    }
}
