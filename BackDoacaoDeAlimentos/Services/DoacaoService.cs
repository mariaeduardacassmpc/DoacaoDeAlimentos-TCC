using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

public class DoacaoService : IDoacaoService
{
    private readonly IDoacaoRepositorio _doacaoRepositorio;

    public DoacaoService(IDoacaoRepositorio doacaoRepositorio)
    {
        _doacaoRepositorio = doacaoRepositorio;
    }

    public async Task AdicionarDoacao(Doacao doacao)
    {
        try
        {
            if (doacao == null)
                throw new ArgumentNullException(nameof(doacao), "A doação não pode ser nula.");

            await _doacaoRepositorio.AdicionarDoacao(doacao);
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
}