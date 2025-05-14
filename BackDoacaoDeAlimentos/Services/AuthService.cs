using TCCDoacaoDeAlimentos.Shared.Models;

public class AuthService
{
    private Usuario _usuarioLogado;

    public bool EstaLogado => _usuarioLogado != null;
    public Usuario Usuario => _usuarioLogado;

    public void Logar(Usuario usuario)
    {
        _usuarioLogado = usuario;
    }

    public void Deslogar()
    {
        _usuarioLogado = null;
    }
}
