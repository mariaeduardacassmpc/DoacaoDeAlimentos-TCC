using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

public interface IDoacaoService
{
    Task<IEnumerable<Doacao>> ObteerTodasDoacoes();
    //Task Doacao ObterDoacaoPorId(int id);
    //Task CriarAsync(CriarDoacaoDto dto);
    //Task AtualizarAsync(int id, AtualizarDoacaoDto dto);
    //Task DeletarAsync(int id);
}
