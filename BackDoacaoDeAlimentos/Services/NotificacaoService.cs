using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Repositorio;
using FrontDoacaoDeAlimentos.Models;
using MimeKit;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMailService _mailServico;
        private readonly IJwtService _jwtService;
        private readonly IAlimentoDoacaoRepositorio _alimentoDoacaoRepositorio;
        private readonly IAlimentoRepositorio _alimentoRepositorio;

        public NotificacaoService
        (
            INotificacaoRepositorio notificacaoRepositorio, 
            IUsuarioRepositorio usuarioRepositorio, 
            IMailService mailServico, 
            IJwtService jwtService,
            IAlimentoDoacaoRepositorio alimentoDoacaoRepositorio,
            IAlimentoRepositorio alimentoRepositorio
        )
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mailServico = mailServico;
            _jwtService = jwtService;
            _alimentoDoacaoRepositorio = alimentoDoacaoRepositorio;
            _alimentoRepositorio = alimentoRepositorio;
        }

        public async Task<bool> EnviarRecuperacaoSenha(string email)
        {
            try
            {
                Usuario usuario = await _usuarioRepositorio.ObterPorEmail(email);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                Entidade entidade = usuario.Entidade;
                if (entidade == null)
                    throw new Exception("Entidade não encontrada.");

                var token = _jwtService.GerarTokenRecuperacao(email);
                var link = $"https://localhost:7170/alterarsenha?token={token}";

                return await _mailServico.EnviarEmailRecuperacaoSenha(
                    entidade.RazaoSocial,
                    usuario.Email,
                    link
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar recuperação de senha.", ex);
            }
        }

            public async Task EnviarEmailConfirmacaoDoacaoDoador(Doacao doacao)
            {
                try
                {
                    var usuario = await _usuarioRepositorio.ObterPorId(doacao.IdDoador);
                    if (usuario == null)
                        throw new Exception("Usuário doador não encontrado.");

                    var instituicao = await _usuarioRepositorio.ObterPorEntidadeId(doacao.IdOng);

                    var alimentosDoacao = await _alimentoDoacaoRepositorio.ObterPorDoacao(doacao.IdDoacao);
                    var nomesAlimentos = new List<string>();

                    foreach (var alimentoD in alimentosDoacao)
                    {
                        var alimento = await _alimentoRepositorio.ObterAlimentoPorId(alimentoD.AlimentoId);
                        if (alimento != null)
                            nomesAlimentos.Add(alimento.Nome);
                    }

                    string nomesAlimentosStr = nomesAlimentos.Count > 0 ? string.Join(", ", nomesAlimentos) : "Alimento";


                    await _mailServico.EnviarEmailConfirmacaoDoacaoDoador(
                        usuario.RazaoSocial,
                        usuario.Email,
                        nomesAlimentosStr,
                        instituicao.RazaoSocial,
                        instituicao.Endereco,
                        instituicao.Telefone,
                        instituicao.Responsavel
                    );
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao enviar confirmação de doação.", ex);
                }
            }

        public async Task EnviarEmailConfirmacaoDoacaoOng(Doacao doacao)
        {
            try
            {
                var usuarioInstituicao = await _usuarioRepositorio.ObterPorEntidadeId(doacao.IdOng);
            }

            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar confirmação de doação.", ex);
            }
        }
    }
}