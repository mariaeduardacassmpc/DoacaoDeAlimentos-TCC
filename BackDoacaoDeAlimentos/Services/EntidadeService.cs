using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;

        public EntidadeService(IEntidadeRepositorio ongRepositorio)
        {
            _entidadeRepositorio = ongRepositorio;
        }

        public async Task AdicionarOng(Entidade ong)
        {
            await _entidadeRepositorio.AdicionarOng(ong);
        }

        public async Task<Entidade> ObterOngPorId(int id)
        {
           return await _entidadeRepositorio.ObterOngPorId(id);
        }

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            return await _entidadeRepositorio.ObterTodasOngs();
        }

        public async Task AtualizarOng(Entidade ong)
        {
            await _entidadeRepositorio.AtualizarOng(ong);
        }

        public async Task DeletarOng(int id)
        {
            await _entidadeRepositorio.DeletarOng(id);
        }
    }
}
