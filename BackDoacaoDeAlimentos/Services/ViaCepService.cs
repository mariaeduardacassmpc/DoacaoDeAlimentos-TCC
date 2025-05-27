namespace BackDoacaoDeAlimentos.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepResponse> BuscarEnderecoPorCep(string cep)
        {
            cep = cep.Replace("-", "").Trim();

            var response = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

            if (response is null || response.Erro == "true")
                throw new Exception("CEP não encontrado.");

            return response;
        }
    }
}
