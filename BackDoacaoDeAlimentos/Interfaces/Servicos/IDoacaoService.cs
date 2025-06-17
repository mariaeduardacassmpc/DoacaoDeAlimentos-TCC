using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

public interface IDoacaoService
{
    Task<IEnumerable<Doacao>> ObterTodasDoacoes();
    Task<IEnumerable<Doacao>> ObterDoacoesComFiltro(FiltroDoacaoDto filtroDoacaoDto);
    Task<Doacao> ObterDoacaoPorId(int id);
    Task<bool> AdicionarDoacao(Doacao doacao);
    Task AtualizarDoacao(Doacao doacao);
    Task<bool> CancelarDoacao(int id, string motivoCancelamento);
    Task<EstatisticasDto> ObterEstatisticas();
}