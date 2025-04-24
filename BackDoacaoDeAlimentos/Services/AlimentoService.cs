using System.Collections.Generic;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.API.Repositories;
using TCCDoacaoDeAlimentos.Shared;

namespace TCCDoacaoDeAlimentos.API.Services
{
    public class AlimentoService : IAlimentoService
    {
        private readonly IAlimentoRepository _repository;

        public AlimentoService(IAlimentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Alimento>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Alimento alimento)
        {
            // Aqui pode incluir regras de negócio antes de adicionar
            await _repository.AddAsync(alimento);
        }

        public async Task RemoveAsync(int id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}
