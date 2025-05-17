using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

public interface IDoacaoRepositorio
{
    Task AdicionarDoacao(Doacao doacao);
    Task<Doacao> ObterDoacaoPorId(int id);
    Task<IEnumerable<Doacao>> ObterDoacoesPorDoadorOuOng(FiltroDoacaoDto filtroDoacaoDto);
    Task<IEnumerable<Doacao>> ObterTodasDoacao();
    Task AtualizarDoacao(Doacao doacao);
    Task DeletarDoacao(int id);
    Task<EstatisticasDto> ObterEstatisticas();
}
