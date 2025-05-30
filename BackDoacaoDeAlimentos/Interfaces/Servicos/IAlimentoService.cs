using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Servicos
{
    public interface IAlimentoService
    {
        Task AdicionarAlimento(Alimento alimento);
        Task<Alimento> ObterAlimentoPorId(int id);
        Task<IEnumerable<Alimento>> ObterTodosAlimentos();
        Task AtualizarAlimento(Alimento alimento);
        Task InativarAlimento(int id);
    }
}
