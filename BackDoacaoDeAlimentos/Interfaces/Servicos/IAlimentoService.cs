using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IAlimentoService
    {
        Task<IEnumerable<Alimento>> ObterTodosAlimentos();
        Task<Alimento> ObterAlimentoPorId(int id);
        Alimento AdicionarAlimento(Alimento alimento);
        void RemoverAlimento(int id);
    }
}
