using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
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
                Console.WriteLine($"Adicionando entidade no service: {entidade.RazaoSocial}"); // Debug

                if (entidade == null)
                    throw new ArgumentNullException(nameof(entidade));

                if (string.IsNullOrWhiteSpace(entidade.CNPJ_CPF))
                    throw new ArgumentException("CNPJ/CPF é obrigatório");

                var existe = await _entidadeRepositorio.VerificarCnpjExistente(entidade.CNPJ_CPF);
                if (existe)
                    throw new InvalidOperationException("Documento já cadastrado");

                await _entidadeRepositorio.AdicionarEntidade(entidade);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no service: {ex.ToString()}"); // Debug
                throw;
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

        public async Task<IEnumerable<Entidade>> BuscarOngsPorCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade inválida.");

            return await _entidadeRepositorio.BuscarOngsPorCidade(cidade);
        }

        public async Task<bool> VerificarCpfCnpjExistente(string documento)
        {
            return await _entidadeRepositorio.VerificarCnpjExistente(documento);
        }
    }
}
