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

    public async Task<bool> EnviarEmailRecuperacaoSenha(string nome, string usuario, string link)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");

            var mensagem = CriarMensagemRecuperacao(
                settings["Email"],
                nome,
                usuario,
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
            _logger.LogError(ex, "Erro ao enviar e-mail de recuperação de senha: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> EnviarEmailConfirmacaoDoacaoDoador(string nomeDoador, string emailDoador, string alimento,
        string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");
            var remetente = settings["Email"];

            var mensagem = CriarMensagemConfirmacaoDoacaoDoacao(
                remetente,
                nomeDoador,
                emailDoador,
                alimento,
                nomeOng,
                enderecoOng,
                telefoneOng,
                responsavelOng
            );

            await EnviarEmail(
                remetente,
                settings["Password"],
                settings["Host"],
                settings.GetValue<int>("Port"),
                mensagem
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail de confirmação de doação: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> EnviarEmailConfirmacaoDoacaoOng(string nomeDoador, string emailDoador, string alimento,
        string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");
            var remetente = settings["Email"];

            var mensagem = CriarMensagemConfirmacaoDoacaoDoacao(
                remetente,
                nomeDoador,
                emailDoador,
                alimento,
                nomeOng,
                enderecoOng,
                telefoneOng,
                responsavelOng
            );

            await EnviarEmail(
                remetente,
                settings["Password"],
                settings["Host"],
                settings.GetValue<int>("Port"),
                mensagem
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail de confirmação de doação: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> EnviarEmailCancelamentoDoacao(string nomeDoador, string emailDoador, string alimento,
        string nomeOng, string telefoneDoador, string motivoCancelamento)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");
            var remetente = settings["Email"];

            //var mensagem = CriarMensagemCancelamentoDoacao(
            //    remetente,
            //    nomeDoador,
            //    emailDoador,
            //    alimento,
            //    telefoneDoador,
            //    motivoCancelamento
            //);

            //await EnviarEmail(
            //    remetente,
            //    settings["Password"],
            //    settings["Host"],
            //    settings.GetValue<int>("Port")
            //    //mensagem
            //);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail de confirmação de doação: " + ex.Message);
            return false;
        }
    }

    public MimeMessage CriarMensagemRecuperacao(string remetente, string nome, string usuario, string link)
    {
        try
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Suporte AlimentAção", remetente));
            mail.To.Add(new MailboxAddress(nome, usuario));
            mail.Subject = "Recuperação de Senha";

            var body = new BodyBuilder
            {
                HtmlBody = $@"
                <p>Olá {nome},</p>
                <p>Recebemos uma solicitação para redefinir a senha da sua conta (<strong>{usuario}</strong>).</p>
                <p>Para continuar, clique no link abaixo:</p>
                <p><a href='{link}'>Redefinir minha senha</a></p>
                <p>Se você não solicitou essa alteração, pode ignorar este e-mail com segurança.</p>
                <p>Atenciosamente,<br/>Equipe de Suporte AlimentAção</p>"
            };


            mail.Body = body.ToMessageBody();
            return mail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar mensagem de recuperação de senha.");
            throw;
        }
    }

    public MimeMessage CriarMensagemConfirmacaoDoacaoDoacao(string remetente, string nomeDoador, string emailDestino,
        string alimento, string nomeOng, string enderecoOng, string telefoneOng, string responsavelOng)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar mensagem de confirmação de doação.");
            throw;
        }
    }

    public MimeMessage CriarMensagemConfirmacaoDoacaoOng(string remetente, string nomeOng, string emailDestino,
        string nomeDoador, string alimento, string telefoneDoador)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar mensagem de notificação para ONG.");
            throw;
        }
    }

    public MimeMessage CriarMensagemCancelamentoDoacao(string remetente, string nomeOng, string emailDestino,
        string nomeDoador, string alimento, string telefoneDoador, string motivoCancelamento)
    {
        try
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Suporte AlimentAção", remetente));
            mail.To.Add(new MailboxAddress(nomeOng, emailDestino));
            mail.Subject = "Nova Doação Recebida";

            var body = new BodyBuilder
            {
                HtmlBody = $@"
                <p>Olá {nomeOng},</p>
                <p>Informamos que o doador <strong>{nomeDoador}</strong> cancelou a doação do alimento <strong>{alimento}</strong>.</p>
                <p><strong>Motivo do cancelamento:</strong> {motivoCancelamento}</p>
                <p>Telefone de contato do doador (caso deseje falar com ele): <strong>{telefoneDoador}</strong></p>
                <p>Essa doação não será mais exibida entre as doações ativas no sistema.</p>
                <br/>
                <p>Atenciosamente,</p>
                <p><strong>Equipe AlimentAção</strong></p>"
            };


            mail.Body = body.ToMessageBody();
            return mail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar mensagem de notificação para ONG.");
            throw;
        }
    }

    public async Task EnviarEmail(string email, string senha, string host, int port, MimeMessage mensagem)
    {
        try
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.Auto);
            await smtp.AuthenticateAsync(email, senha);
            await smtp.SendAsync(mensagem);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail via SMTP.");
            throw;
        }
    }
}
