using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using System.Transactions;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

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
        try
        {
            if (entidade == null)
                throw new Exception("Entidade não pode ser nula.");

            if (usuario == null)
                throw new Exception("Usuário não pode ser nulo.");

            var existe = await _entidadeRepositorio.VerificaDocumentoeEmailExistente(entidade.CNPJ_CPF, entidade.Email);
            if (existe)
                throw new Exception("Já existe um cadastro com este CPF/CNPJ ou E-mail.");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var entidadeId = await _entidadeRepositorio.AdicionarEntidade(entidade);
            usuario.EntidadeId = entidadeId;

            await _usuarioService.RegistrarUsuario(usuario);

            scope.Complete();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao adicionar entidade.");
        }
    }

    public async Task<Entidade> ObterEntidadePorId(int id)
    {
        try
        {
            var entidade = await _entidadeRepositorio.ObterEntidadePorId(id);
            if (entidade == null)
                throw new Exception($"Entidade com ID {id} não encontrada.");
            return entidade;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter entidade por ID.");
        }
    }

    public async Task<IEnumerable<Entidade>> ObterTodasInstituicoes()
    {
        try
        {
            return await _entidadeRepositorio.ObterTodasInstituicoes();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as instituições.");
        }
    }

    public async Task<IEnumerable<Entidade>> ObterNomesFantasiaDasInstituicoesQueDoadorDoou(int id)
    {
        try
        {
            return await _entidadeRepositorio.ObterNomesFantasiaDasInstituicoesQueDoadorDoou(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as instituições.");
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
            throw new Exception("Erro ao obter todas as entidades.");
        }
    }

    public async Task<IEnumerable<Entidade>> ObterInstituicaoPorCidade(string cidade)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new Exception("Cidade não pode ser vazia.");

            return await _entidadeRepositorio.ObterOngsPorCidade(cidade);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter instituições por cidade.");
        }
    }

    public async Task AtualizarEntidade(EntidadeEdicaoDto entidade)
    {
        try
        {
            if (entidade == null)
                throw new Exception("Entidade para atualização não pode ser nula.");

            await _entidadeRepositorio.AtualizarEntidade(entidade);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar entidade.");
        }
    }

    public async Task InativarEntidade(int id)
    {
        try
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _entidadeRepositorio.InativarEntidade(id);

            scope.Complete();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao inativar entidade.");
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
            throw new Exception("Erro ao verificar documento e email.");
        }
    }
}