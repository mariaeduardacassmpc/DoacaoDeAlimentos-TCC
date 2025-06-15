namespace BackDoacaoDeAlimentos.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCep> BuscarEnderecoPorCep(string cep)
        {
            cep = cep.Replace("-", "").Trim(); // Remove hífens e espaços desnecessários

            var response = await _httpClient.GetFromJsonAsync<ViaCep>($"https://viacep.com.br/ws/{cep}/json/");

            if (response is null || response.Erro == "true")
                throw new Exception("CEP não encontrado.");

            return response;
        }
    }
}
