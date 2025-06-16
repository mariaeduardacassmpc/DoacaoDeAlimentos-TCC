namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class EntidadeEdicaoDto
    {
        public int Id { get; set; }
        public string TpEntidade { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;

        public string CNPJ_CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Sexo { get; set; } = string.Empty;         
        public string? Responsavel { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
