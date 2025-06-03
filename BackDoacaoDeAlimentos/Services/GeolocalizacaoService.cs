using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using System.Globalization;

public class GeocodingService : IGeolocalizacaoService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "fd828fd9cabc4e51bc7c6155f0588844";

    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AlimentAcao/1.0 (alimentacao@gmail.com)");
    }

    public async Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude)
    {
        var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}&lon={longitude.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            await ObterCidadePorCoordenadasOpenCage(latitude, longitude);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<NominatimResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data?.Address?.City;
    }

    public async Task<string?> ObterCidadePorCoordenadasOpenCage(double latitude, double longitude)
    {
        var url = $"https://api.opencagedata.com/geocode/v1/json?q={latitude.ToString(CultureInfo.InvariantCulture)}+{longitude.ToString(CultureInfo.InvariantCulture)}&key={_apiKey}&language=pt&pretty=1";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro na chamada OpenCage: {response.StatusCode}");
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<OpenCageResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var components = data?.Results?.FirstOrDefault()?.Components;

        return components?.City;
    }
}
