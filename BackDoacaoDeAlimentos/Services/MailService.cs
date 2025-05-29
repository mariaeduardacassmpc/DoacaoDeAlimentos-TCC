using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using MailKit.Net.Smtp; 

public class MailService : IMailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<MailService> _logger;

    public MailService(IConfiguration config, ILogger<MailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> EnviarEmailRecuperacaoSenha(string nome, string usuario, string emailDestino, string link)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");

            var mensagem = CriarMensagemRecuperacao(
                settings["Email"],
                nome,
                usuario,
                emailDestino,
                link
            );

            await EnviarEmail(
                settings["Email"],
                settings["Password"],
                settings["Host"],
                settings.GetValue<int>("Port"),
                mensagem
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail: " + ex.Message);
            return false;
        }
    }

    public MimeMessage CriarMensagemRecuperacao(string remetente, string nome, string usuario, string destino, string link)
    {
        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress("Suporte AlimentAção", remetente));
        mail.To.Add(new MailboxAddress(nome, destino));
        mail.Subject = "Recuperação de Senha";

        var body = new BodyBuilder
        {
            HtmlBody = $@"<p>Olá {nome},</p>
                <p><a href='{link}'>Redefinir senha</a> para o usuário: {usuario}</p>"
        };

        mail.Body = body.ToMessageBody();
        return mail;
    }

    public MimeMessage CriarMensagemConfirmacaoDoacao(string remetente, string nomeDoador, string emailDestino, 
        string alimento, string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng)
    {
        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress("Suporte AlimentAção", remetente));
        mail.To.Add(new MailboxAddress(nomeDoador, emailDestino));
        mail.Subject = "Confirmação de Doação Realizada";

        var body = new BodyBuilder
        {
            HtmlBody = $@"
                <p>Olá {nomeDoador},</p>
                <p>Parabéns! Sua doação do alimento <strong>{alimento}</strong> foi efetuada para a ONG <strong>{nomeOng}</strong>.</p>
                <p>Endereço da ONG: <strong>{enderecoOng}</strong></p>
                <p>Telefone de contato: <strong>{telefoneOng}</strong></p>
                <p>Responsável: <strong>{responsavelOng}</strong></p>
                <p>Aguarde a ONG entrar em contato para finalizar o processo.</p>
                <p>Obrigado por sua colaboração!</p>"
        };

        mail.Body = body.ToMessageBody();
        return mail;
    }

    public MimeMessage CriarMensagemNotificacaoOng(string remetente, string nomeOng, string emailDestino, 
        string nomeDoador, string alimento, string telefoneDoador)
    {
        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress("Suporte AlimentAção", remetente));
        mail.To.Add(new MailboxAddress(nomeOng, emailDestino));
        mail.Subject = "Nova Doação Recebida";

        var body = new BodyBuilder
        {
            HtmlBody = $@"
                <p>Olá {nomeOng},</p>
                <p>O doador <strong>{nomeDoador}</strong> realizou uma doação do alimento <strong>{alimento}</strong>.</p>
                <p>Telefone para contato do doador: <strong>{telefoneDoador}</strong></p>
                <p>Por favor, entre em contato com o doador para combinar os detalhes da entrega.</p>
                <p>Equipe AlimentAção.</p>"
        };

        mail.Body = body.ToMessageBody();
        return mail;
    }

    public async Task EnviarEmail(string email, string senha, string host, int port, MimeMessage mensagem)
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(email, senha);
        await smtp.SendAsync(mensagem);
        await smtp.DisconnectAsync(true);
    }
}