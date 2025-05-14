public class SessaoUsuarioService
{
    private UsuarioLogado _usuario;

    public void Login(UsuarioLogado usuario)
    {
        _usuario = usuario;
    }

    public UsuarioLogado ObterUsuario()
    {
        return _usuario;
    }

    public bool EstaLogado => _usuario != null;

    public void Logout()
    {
        _usuario = null;
    }
}
