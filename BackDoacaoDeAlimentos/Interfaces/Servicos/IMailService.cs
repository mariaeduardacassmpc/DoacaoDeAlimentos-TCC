using MimeKit;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IMailService
    {
        Task<bool> EnviarEmailRecuperacaoSenha(string nome, string usuario, string link);
        Task<bool> EnviarEmailConfirmacaoDoacaoDoador(string nomeDoador, string emailDoador, string alimento, string nomeOng,
        string enderecoOng, string telefoneOng, string responsavelOng);
        Task<bool> EnviarEmailConfirmacaoDoacaoOng(string nomeDoador, string emailDoador, string alimento,
        string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng);
        Task<bool> EnviarEmailCancelamentoDoacao(string nomeDoador, string emailDoador, string alimento,
        string nomeOng, string telefoneDoador, string motivoCancelamento);
        MimeMessage CriarMensagemRecuperacao(string remetente, string nome, string usuario, string link);
        MimeMessage CriarMensagemConfirmacaoDoacaoDoacao(string remetente, string nomeDoador, string emailDestino,
          string alimento, string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng);
        MimeMessage CriarMensagemConfirmacaoDoacaoOng(string remetente, string nomeOng, string emailDestino,
        string nomeDoador, string alimento, string telefoneDoador);
    }
}
