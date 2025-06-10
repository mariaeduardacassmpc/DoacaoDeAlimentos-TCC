using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

public class DoacaoService : IDoacaoService
{
    private readonly IDoacaoRepositorio _doacaoRepositorio;
    private readonly IAutenticacaoService _autenticacaoService;
    private readonly INotificacaoService _notificacaoService;

    public DoacaoService(IDoacaoRepositorio doacaoRepositorio, IAutenticacaoService autenticacaoService, INotificacaoService notificacaoService)
    {
        _doacaoRepositorio = doacaoRepositorio;
        _autenticacaoService = autenticacaoService;
        _notificacaoService = notificacaoService;
    }

    public async Task<bool> AdicionarDoacao(Doacao doacao)
    {
        try
        {
            if (doacao == null)
                throw new ArgumentNullException(nameof(doacao), "A doação não pode ser nula.");

            await _doacaoRepositorio.AdicionarDoacao(doacao);
            if (doacao.IdDoacao > 0)
            {
                await _notificacaoService.EnviarEmailConfirmacaoDoacaoDoador(doacao);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao tentar adicionar a doação. Por favor, verifique os dados e tente novamente.", ex);
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

    public async Task DeletarDoacao(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("O ID da doação deve ser maior que zero.", nameof(id));

            await _doacaoRepositorio.DeletarDoacao(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível deletar a doação com ID {id}. Por favor, verifique o ID e tente novamente.", ex);
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