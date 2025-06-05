using System.Transactions;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;

namespace BackDoacaoDeAlimentos.Servicos
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly IUsuarioService _usuarioService;


        public EntidadeService(IEntidadeRepositorio entidadeRepositorio, IUsuarioService usuarioService)
        {
            _entidadeRepositorio = entidadeRepositorio;
            _usuarioService = usuarioService;
        }

        public async Task AdicionarEntidade(Entidade entidade, Usuario usuario)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade));

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            var existe = await _entidadeRepositorio.VerificaDocumentoeEmailExistente(entidade.CNPJ_CPF, entidade.Email);
            if (existe)
                throw new InvalidOperationException("Já existe um cadastro com este CPF/CNPJ ou E-mail.");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var entidadeId = await _entidadeRepositorio.AdicionarEntidade(entidade);
            usuario.EntidadeId = entidadeId;

            await _usuarioService.RegistrarUsuario(usuario);

            scope.Complete();
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
            return await _entidadeRepositorio.ObterEntidadePorId(id)
                ?? throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");
        }

        public async Task<IEnumerable<Entidade>> ObterTodasInstituicoes()
        {
            return await _entidadeRepositorio.ObterTodasInstituicoes();
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            return await _entidadeRepositorio.ObterTodasEntidades();
        }

        public async Task<IEnumerable<Entidade>> ObterInstituicaoPorCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade não pode ser vazia.", nameof(cidade));

            return await _entidadeRepositorio.ObterOngsPorCidade(cidade);
        }

        public async Task AtualizarEntidade(EntidadeEdicaoDto entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade));

            await _entidadeRepositorio.AtualizarEntidade(entidade);
        }

        public async Task InativarEntidade(int id)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _entidadeRepositorio.InativarEntidade(id);

            scope.Complete();
        }

        public async Task<bool> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            return await _entidadeRepositorio.VerificaDocumentoeEmailExistente(documento, email);
        }
    }
}
