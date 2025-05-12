using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using FrontDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public NotificacaoService(INotificacaoRepositorio notificacaoRepositorio)
        {
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public async Task<IEnumerable<Notificacao>> ObterTodasNotificacoes()
        {
            try
            {
                return await _notificacaoRepositorio.ObterTodasNotificacoes();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todas as notificações.", ex);
            }
        }

        public async Task<Notificacao> ObterNotificacaoPorId(int id)
        {
            try
            {
                return await _notificacaoRepositorio.ObterNotificacaoPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter a notificação com ID {id}.", ex);
            }
        }

        public async Task<Notificacao> AdicionarNotificacao(Notificacao notificacao)
        {
            try
            {
                if (notificacao == null)
                    throw new ArgumentNullException(nameof(notificacao));

                return await _notificacaoRepositorio.AdicionarNotificacao(notificacao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar a notificação.", ex);
            }
        }
       
        public async Task RemoverNotificacao(int id)
        {
            try
            {
                await _notificacaoRepositorio.DeletarNotificacao(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover a notificação com ID {id}.", ex);
            }
        }
    }
}