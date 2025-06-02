using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Dto;

public class GeolocalizacaoService : IGeolocalizacaoService
{
    private readonly HttpClient _httpClient;

    public GeolocalizacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "SeuAppDeDoacao/1.0 (seuemail@seudominio.com)");
    }

    public async Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude)
    {
        var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&lon={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro na chamada Nominatim: {response.StatusCode}");
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<NominatimResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data?.Address?.City
            ?? data?.Address?.Town
            ?? data?.Address?.Village
            ?? data?.Address?.Municipality;
    }

}
