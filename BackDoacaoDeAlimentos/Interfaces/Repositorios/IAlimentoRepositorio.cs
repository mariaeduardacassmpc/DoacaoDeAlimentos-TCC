using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Interfaces.Repositorios;

public interface IAlimentoRepositorio
{
    IEnumerable<Alimento> ObterTodosAlimentos();
    Alimento ObterAlimentoPorId(int id);
    Alimento GravarAlimento(Alimento alimento);
    void RemoverAlimento(int id);
}
