//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TCCDoacaoDeAlimentos.Shared.Models;

//public class DoacaoService : IDoacaoService
//{
//    private readonly IDoacaoRepositorio _doacaoRepositorio;

//    public DoacaoService(IDoacaoRepositorio doacaoRepositorio)
//    {
//        _doacaoRepositorio = doacaoRepositorio;
//    }

//    public async Task<IEnumerable<Doacao>> ObterTodasDoacoes()
//    {
//        var doacoes = await _doacaoRepositorio.ObterTodasAsync();
//        return doacoes.Select(d => new Doacao
//        {
//            Id = d.Id,
//            NomeAlimento = d.NomeAlimento,
//            Quantidade = d.Quantidade,
//            Data = d.Data
//        });
//    }

//    public async Task<DoacaoDto> ObterPorIdAsync(int id)
//    {
//        var doacao = await _doacaoRepository.ObterPorIdAsync(id);
//        if (doacao == null) return null;

//        return new DoacaoDto
//        {
//            Id = doacao.Id,
//            NomeAlimento = doacao.NomeAlimento,
//            Quantidade = doacao.Quantidade,
//            Data = doacao.Data
//        };
//    }

//    public async Task<int> CriarAsync(CriarDoacaoDto dto)
//    {
//        var doacao = new Doacao(dto.NomeAlimento, dto.Quantidade, dto.Data);
//        await _doacaoRepository.AdicionarAsync(doacao);
//        return doacao.Id;
//    }

//    public async Task<bool> AtualizarAsync(int id, AtualizarDoacaoDto dto)
//    {
//        var doacao = await _doacaoRepository.ObterPorIdAsync(id);
//        if (doacao == null) return false;

//        doacao.Atualizar(dto.NomeAlimento, dto.Quantidade, dto.Data);
//        await _doacaoRepository.AtualizarAsync(doacao);
//        return true;
//    }

//    public async Task<bool> DeletarAsync(int id)
//    {
//        var doacao = await _doacaoRepository.ObterPorIdAsync(id);
//        if (doacao == null) return false;

//        await _doacaoRepository.DeletarAsync(doacao);
//        return true;
//    }
//}
