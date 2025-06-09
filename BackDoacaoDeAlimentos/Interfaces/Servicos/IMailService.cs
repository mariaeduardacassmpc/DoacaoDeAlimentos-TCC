using MimeKit;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IMailService
    {
        Task<bool> EnviarEmailRecuperacaoSenha(string nome, string usuario, string link);
        MimeMessage CriarMensagemRecuperacao(string remetente, string nome, string usuario, string link);
        MimeMessage CriarMensagemConfirmacaoDoacao(string remetente, string nomeDoador, string emailDestino,
          string alimento, string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng);
        MimeMessage CriarMensagemNotificacaoOng(string remetente, string nomeOng, string emailDestino,
        string nomeDoador, string alimento, string telefoneDoador);
    }
}
