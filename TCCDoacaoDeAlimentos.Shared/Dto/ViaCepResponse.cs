public class ViaCepResponse
{
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Localidade { get; set; } // Cidade
    public string Uf { get; set; }         // Estado
    public string Erro { get; set; }
}
