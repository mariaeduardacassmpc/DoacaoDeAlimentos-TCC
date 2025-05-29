namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IAutenticacaoService
    {
        Task<bool> EnviarRecuperacaoSenha(string email); 
    }
}
