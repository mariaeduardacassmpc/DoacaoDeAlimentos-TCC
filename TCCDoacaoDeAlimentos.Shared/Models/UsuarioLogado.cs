public class UsuarioLogado
{
    public int UsuarioId { get; set; }
    public int EntidadeId { get; set; }
    public string Email { get; set; }
    public string TipoEntidade { get; set; } // "F", "J", "O"
    public string Token { get; set; }

}
