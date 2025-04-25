using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios;

public interface IAlimentoRepositorio
{
    Task<IEnumerable<Alimento>> ObterTodosAlimentos();
    Task<Alimento> ObterAlimentoPorId(int id);
    Task AdicionarAlimento(Alimento alimento);
    Task RemoverAlimento(int id);
}
