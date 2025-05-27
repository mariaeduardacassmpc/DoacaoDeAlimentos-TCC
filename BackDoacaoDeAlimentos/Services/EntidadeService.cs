using System.Transactions;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;


        public EntidadeService(IEntidadeRepositorio entidadeRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _entidadeRepositorio = entidadeRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task AdicionarEntidade(Entidade entidade, Usuario usuario)
        {
            try
            {
                if (entidade == null)
                    throw new ArgumentNullException(nameof(entidade));

                if (string.IsNullOrWhiteSpace(entidade.CNPJ_CPF))
                    throw new ArgumentException("CNPJ/CPF é obrigatório");

                if (entidade.Senha != entidade.ConfirmarSenha)
                    throw new ArgumentException("As senhas não coincidem");

                var existe = await _entidadeRepositorio.VerificarCnpjExistente(entidade.CNPJ_CPF);
                if (existe)
                    throw new InvalidOperationException(entidade.Tipo == Entidade.TipoEntidade.F ? "CPF já cadastrado" : "CNPJ já cadastrado");

                var entidadeId = await _entidadeRepositorio.AdicionarEntidade(entidade);
                entidade.Id = entidadeId;

                usuario.EntidadeId = entidadeId;
                await _usuarioRepositorio.Adicionar(usuario);
            }
            catch (Exception ex)
            {
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
                return await _entidadeRepositorio.ObterTodasOngs();
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
