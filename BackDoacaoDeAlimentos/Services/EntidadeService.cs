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
            try
            {
                await _entidadeRepositorio.AdicionarEntidade(entidade);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter Entidade por Id. ", ex);
            }
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
            try
            {
                return await _entidadeRepositorio.ObterEntidadePorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter Entidade por Id. ", ex);
            }
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            try
            {
                return await _entidadeRepositorio.ObterTodasEntidades();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter todas Entidades. ", ex);
            }
        }

        public async Task AtualizarEntidade(Entidade entidade)
        {
            try
            {
                await _entidadeRepositorio.AtualizarEntidade(entidade);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar Entidade. ", ex);
            }
        }

        public async Task DeletarEntidade(int id)
        {
            try
            {
                await _entidadeRepositorio.DeletarEntidade(id);
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao remover Entidade. ", ex);
            }
        }
    }
}
