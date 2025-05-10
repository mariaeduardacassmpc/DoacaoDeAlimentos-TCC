using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Models;
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
        await _doacaoRepositorio.AdicionarDoacao(doacao);
    }

    public async Task<Doacao> ObterDoacaoPorId(int id)
    {
        return await _doacaoRepositorio.ObterDoacaoPorId(id);
    }

    public async Task<IEnumerable<Doacao>> ObterTodasDoacoes()
    {
        return await _doacaoRepositorio.ObterTodasDoacoes();
    }

    public async Task AtualizarDoacao(Doacao doacao)
    {
        await _doacaoRepositorio.AtualizarDoacao(doacao);
    }

    public async Task DeletarDoacao(int id)
    {
        await _doacaoRepositorio.DeletarDoacao(id);
    }
}
