using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Dto;

public class GeolocalizacaoService : IGeolocalizacaoService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "fd828fd9cabc4e51bc7c6155f0588844";

    public GeolocalizacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "SeuAppDeDoacao/1.0 (seuemail@seudominio.com)");
    }

    public async Task<string?> ObterCidadePorCoordenadas(double latitude, double longitude)
    {
        var urlOpenCage = $"https://api.opencagedata.com/geocode/v1/json?q={latitude.ToString(CultureInfo.InvariantCulture)}+{longitude.ToString(CultureInfo.InvariantCulture)}&key={_apiKey}&language=pt&pretty=1";
        var responseOpenCage = await _httpClient.GetAsync(urlOpenCage);

        if (responseOpenCage.IsSuccessStatusCode)
        {
            var jsonOpenCage = await responseOpenCage.Content.ReadAsStringAsync();
            var openCage = JsonSerializer.Deserialize<OpenCageResponse>(jsonOpenCage, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var components = openCage?.Results?.FirstOrDefault()?.Components;
            var cidade = components?.City;
              
            if (!string.IsNullOrEmpty(cidade))
                return cidade;
        }

        var urlNominatim = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude.ToString(CultureInfo.InvariantCulture)}&lon={longitude.ToString(CultureInfo.InvariantCulture)}";
        var responseNominatim = await _httpClient.GetAsync(urlNominatim);

        if (!responseNominatim.IsSuccessStatusCode)
            return null;

        var jsonNominatim = await responseNominatim.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<NominatimResponse>(jsonNominatim, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data?.Address?.City;
    }
}
