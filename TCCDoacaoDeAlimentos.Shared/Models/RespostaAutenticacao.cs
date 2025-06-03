namespace TCCDoacaoDeAlimentos.Shared.Models
{
    public class RespostaAutenticacao 
    {
        public string NomeUsuario { get; set; } = "";
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
