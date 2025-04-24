using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared;

namespace TCCDoacaoDeAlimentos.API.Services
{
    public interface IAlimentoService
    {
        Task<IEnumerable<Alimento>> GetAllAsync();
        Task AddAsync(Alimento alimento);
        Task RemoveAsync(int id);
    }
}
