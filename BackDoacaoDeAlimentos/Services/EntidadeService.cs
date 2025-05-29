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
        private readonly IUsuarioRepositorio _usuarioRepositorio;


        public EntidadeService(IEntidadeRepositorio entidadeRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _entidadeRepositorio = entidadeRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
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
            entidade.Id = entidadeId;
            usuario.EntidadeId = entidadeId;

            await _usuarioRepositorio.Adicionar(usuario);

            scope.Complete();
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
            return await _entidadeRepositorio.ObterEntidadePorId(id)
                ?? throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");
        }

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            return await _entidadeRepositorio.ObterTodasOngs();
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            return await _entidadeRepositorio.ObterTodasEntidades();
        }

        public async Task<IEnumerable<Entidade>> ObterOngsPorCidade(string cidade)
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

        public async Task DeletarEntidade(int id)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _usuarioRepositorio.Remover(id);
            await _entidadeRepositorio.DeletarEntidade(id);

            scope.Complete();
        }

        public async Task<bool> VerificarDocumentoeEmailExistente(string documento, string email)
        {
            return await _entidadeRepositorio.VerificaDocumentoeEmailExistente(documento, email);
        }
    }
}
