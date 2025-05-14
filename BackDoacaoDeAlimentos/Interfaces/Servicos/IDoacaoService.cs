using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

public interface IDoacaoService
{
    Task<IEnumerable<Doacao>> ObterTodasDoacoes();
    Task<IEnumerable<Doacao>> ObterDoacoesComFiltro(FiltroDoacaoDto filtroDoacaoDto);
    Task<Doacao> ObterDoacaoPorId(int id);
    Task AdicionarDoacao(Doacao doacao);
    Task AtualizarDoacao(Doacao doacao);
    Task DeletarDoacao(int id);
}
