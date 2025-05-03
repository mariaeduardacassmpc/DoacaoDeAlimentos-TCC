using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class OngService : IOngService
    {
        private readonly IOngRepositorio _ongRepositorio;

        public OngService(IOngRepositorio ongRepositorio)
        {
            _ongRepositorio = ongRepositorio;
        }

        public async Task AdicionarOng(Entidade ong)
        {
            await _ongRepositorio.AdicionarOng(ong);
        }

        //public async Task<Ong> ObterOngPorId(int id)
        //{
        //    return await _ongRepositorio.ObterOngPorId(id);
        //}

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            return await _ongRepositorio.ObterTodasOngs();
        }

        public async Task AtualizarOng(Entidade ong)
        {
            await _ongRepositorio.AtualizarOng(ong);
        }

        public async Task DeletarOng(int id)
        {
            await _ongRepositorio.DeletarOng(id);
        }
    }
}
