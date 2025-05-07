using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;

        public EntidadeService(IEntidadeRepositorio entidadeRepositorio)
        {
            _entidadeRepositorio = entidadeRepositorio;
        }

        public async Task AdicionarEntidade(Entidade entidade)
        {
            await _entidadeRepositorio.AdicionarEntidade(entidade);
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
           return await _entidadeRepositorio.ObterEntidadePorId(id);
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            return await _entidadeRepositorio.ObterTodasEntidades();
        }

        public async Task AtualizarEntidade(Entidade entidade)
        {
            await _entidadeRepositorio.AtualizarEntidade(entidade);
        }

        public async Task DeletarEntidade(int id)
        {
            await _entidadeRepositorio.DeletarEntidade(id);
        }
    }
}
