using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

public class DoacaoService : IDoacaoService
{
    private readonly IDoacaoRepositorio _doacaoRepositorio;
    private readonly IAutenticacaoService _autenticacaoService;
    private readonly INotificacaoService _notificacaoService;
    private readonly IAlimentoDoacaoService _alimentiDoacaoService;
    private readonly IAlimentoDoacaoRepositorio _alimentiDoacaoRepositorio;

    public DoacaoService
    (
        IDoacaoRepositorio doacaoRepositorio, 
        IAutenticacaoService autenticacaoService, 
        INotificacaoService notificacaoService,
        IAlimentoDoacaoService alimentiDoacaoService,
        IAlimentoDoacaoRepositorio alimentoDoacaoRepositorio

    )
    {
        _doacaoRepositorio = doacaoRepositorio;
        _autenticacaoService = autenticacaoService;
        _notificacaoService = notificacaoService;
        _alimentiDoacaoService = alimentiDoacaoService;
        _alimentiDoacaoRepositorio = alimentoDoacaoRepositorio;
    }


    public async Task<bool> AdicionarDoacao(Doacao doacao)
    {
        try
        {
            if (doacao == null)
                throw new ArgumentNullException(nameof(doacao), "A doação não pode ser nula.");

            var id = await _doacaoRepositorio.AdicionarDoacao(doacao);
            doacao.Id = id;

            if (doacao.Alimentos != null && doacao.Alimentos.Count > 0)
            {
                foreach (var item in doacao.Alimentos)
                {
                    item.IdDoacao = id;
                    await _alimentiDoacaoRepositorio.Adicionar(item);
                }
            }

            if (doacao.Id > 0)
            {
                await _notificacaoService.EnviarEmailConfirmacaoDoacaoDoador(doacao);
                await _notificacaoService.EnviarEmailConfirmacaoDoacaoOng(doacao);

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao tentar adicionar a doação. Por favor, verifique os dados e tente novamente.");
        }
    }

    public async Task<Doacao> ObterDoacaoPorId(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("O ID da doação deve ser maior que zero.", nameof(id));

            return await _doacaoRepositorio.ObterDoacaoPorId(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível obter a doação com ID {id}. Por favor, verifique o ID e tente novamente.", ex);
        }
    }

    public async Task<IEnumerable<Doacao>> ObterTodasDoacoes()
    {
        try
        {
            return await _doacaoRepositorio.ObterTodasDoacao();
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao tentar obter a lista de doações. Por favor, tente novamente mais tarde.", ex);
        }
    }

    public async Task AtualizarDoacao(Doacao doacao)
    {
        try
        {
            if (doacao == null)
                throw new ArgumentNullException(nameof(doacao), "A doação não pode ser nula.");

            await _doacaoRepositorio.AtualizarDoacao(doacao);
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao tentar atualizar a doação. Por favor, verifique os dados e tente novamente.", ex);
        }
    }

    public async Task<bool> CancelarDoacao(int id, string motivoCancelamento)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("O ID da doação deve ser maior que zero.", nameof(id));

            var doacao = await ObterDoacaoPorId(id);
            doacao.Observacao = motivoCancelamento;

            await _doacaoRepositorio.CancelarDoacao(id, motivoCancelamento);
            var doacaoAtualizada = await ObterDoacaoPorId(id);

            if (doacaoAtualizada.Status == StatusDoacao.Cancelada)
            {
                await _notificacaoService.EnviarEmailCancelamentoDoacao(doacao);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possível deletar a doação com ID");
        }
    }

    public async Task<IEnumerable<Doacao>> ObterDoacoesComFiltro(FiltroDoacaoDto filtroDoacaoDto)
    {
        try
        {
            return await _doacaoRepositorio.ObterDoacoesPorDoadorOuOng(filtroDoacaoDto);
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível obter as doações do doador com ID {filtroDoacaoDto}. Por favor, verifique o ID e tente novamente.", ex);
        }
    }

    public async Task<EstatisticasDto> ObterEstatisticas()
    {
        try
        {
            return await _doacaoRepositorio.ObterEstatisticas();
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível obter as estatisticas");
        }
    }
}