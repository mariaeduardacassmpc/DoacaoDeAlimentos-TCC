using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude)
        {
            // Usando Nominatim (OpenStreetMap) para geocodificação reversa
            var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude}&lon={longitude}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            if (doc.RootElement.TryGetProperty("address", out var address))
            {
                if (address.TryGetProperty("city", out var cityProp))
                    return cityProp.GetString();
                if (address.TryGetProperty("town", out var townProp))
                    return townProp.GetString();
                if (address.TryGetProperty("village", out var villageProp))
                    return villageProp.GetString();
            }

            return null;
        }
    }
}
