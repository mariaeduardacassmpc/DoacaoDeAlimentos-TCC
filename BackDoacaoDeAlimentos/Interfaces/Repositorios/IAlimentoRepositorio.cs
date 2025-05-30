using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios;
public interface IAlimentoRepositorio
{
    Task AdicionarAlimento(Alimento alimento);
    Task<Alimento> ObterAlimentoPorId(int id);
    Task<IEnumerable<Alimento>> ObterTodosAlimentos();
    Task AtualizarAlimentos(Alimento alimento);
    Task InativarAlimento(int id);
}
