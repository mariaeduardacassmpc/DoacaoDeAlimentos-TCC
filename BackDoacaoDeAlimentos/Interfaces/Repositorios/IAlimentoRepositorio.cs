using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.

namespace TCCDoacaoDeAlimentos.API.Repositories
{
    public interface IAlimentoRepositorio
    {
        Task<IEnumerable<Alimento>> ObterTodosAlimentos();
        Task<Alimento?> ObterAlimentoPorId(int id);
        Task AdicionarAlimento(Alimento alimento);
        Task RemoverAlimento(int id);
    }
}
