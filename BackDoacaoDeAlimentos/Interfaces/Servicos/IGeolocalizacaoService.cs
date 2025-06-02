namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IGeolocalizacaoService
    {
        Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude);
    }

}
