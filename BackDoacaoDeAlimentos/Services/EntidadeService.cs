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
                
                var existe = await _entidadeRepositorio.VerificaDocumentoeEmailExistente(entidade.CNPJ_CPF, entidade.Email);
                if (existe)
                    throw new InvalidOperationException("Já existe um cadastro com este CPF/CNPJ ou E-mail.");
                var entidadeId = await _entidadeRepositorio.AdicionarEntidade(entidade);
                entidade.Id = entidadeId;
                usuario.EntidadeId = entidadeId;

                await _usuarioRepositorio.Adicionar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao Cadastrar.", ex);
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

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
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
            try
            {
                return await _entidadeRepositorio.BuscarOngsPorCidade(cidade);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cidade inválida. ", ex);
            }
        }

        public async Task<bool> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            try
            {
                return await _entidadeRepositorio.VerificaDocumentoeEmailExistente(documento, email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar CPF/CNPJ ou Email. ", ex);
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
    }
}
