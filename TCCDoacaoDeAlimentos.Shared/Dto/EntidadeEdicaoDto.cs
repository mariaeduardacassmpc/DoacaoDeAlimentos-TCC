namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class EntidadeEdicaoDto
    {
        public int Id { get; set; }
        public string TpEntidade { get; set; } = string.Empty;   // F, J ou O
        public string RazaoSocial { get; set; } = string.Empty;
        public string CNPJ_CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;         // M, F ou outro
        public string Responsavel { get; set; } = string.Empty;
        public string TpPessoa { get; set; } = string.Empty;     // F ou J
        public bool Ativo;
    }
}
