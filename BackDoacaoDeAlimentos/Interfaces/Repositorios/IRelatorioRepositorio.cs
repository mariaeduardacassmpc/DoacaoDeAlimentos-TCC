using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios
{
    public interface IRelatorioRepositorio
    {
        Task<int> ObterTotalDoacoesMes(int id);
        Task<int> ObterTotalUsuarios();
        Task<IEnumerable<AlimentoPorCategoriaDto>> ObterAlimentosPorCategoria(int id);
        Task<double> ObterTotalKgAlimentos(int id);
    }
}
