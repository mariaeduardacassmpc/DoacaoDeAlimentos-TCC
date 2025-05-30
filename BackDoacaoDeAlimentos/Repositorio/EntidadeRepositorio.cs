using Dapper;
using System.Data;
using System.Data.Common;
using TCCDoacaoDeAlimentos.Shared.Dto;
using BackDoacaoDeAlimentos.Repositorio;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;

namespace BackDoacaoDeAlimentos.Repositorios
{
    public class EntidadeRepositorio : IEntidadeRepositorio
    {
        private readonly IDbConnection _db;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public EntidadeRepositorio(IDbConnection db, IUsuarioRepositorio usuarioRepositorio)
        {
            _db = db;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IEnumerable<Entidade>> ObterOngsPorCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade não pode ser vazia.", nameof(cidade));

            try
            {
                var sql = @"SELECT * 
                    FROM Entidade 
                    WHERE Cidade = @Cidade 
                    AND TpEntidade = 'O'";

                return await _db.QueryAsync<Entidade>(sql, new { Cidade = cidade });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter ONGs por cidade: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            try
            {
                const string sql = "SELECT * FROM Entidade WHERE TpEntidade = 'O'";
                return await _db.QueryAsync<Entidade>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todas as ONGs: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            try
            {
                const string sql = "SELECT * FROM Entidade";
                return await _db.QueryAsync<Entidade>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todas as entidades: " + ex.Message, ex);
            }
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.", nameof(id));

            try
            {
                const string sql = "SELECT * FROM Entidade WHERE Id = @Id";
                return await _db.QueryFirstOrDefaultAsync<Entidade>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter entidade por Id: " + ex.Message, ex);
            }
        }

        public async Task AdicionarEntidade(Entidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade));

            using var transaction = _db.BeginTransaction();

            try
            {
                var existeUsuario = await VerificaDocumentoeEmailExistente(entidade.CNPJ_CPF, entidade.Email);

                const string sql = @"
                    INSERT INTO Entidade (
                            TpEntidade, RazaoSocial, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, Email, Sexo, Responsavel, Latitude, Altitude, Numero, TpPessoa
                    ) VALUES (
                            @TpEntidade, @RazaoSocial, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, @Email, @Sexo, @Responsavel, @Latitude, @Altitude, @Numero, @TpPessoa
                        );
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";

                var entidadeId = await _db.ExecuteScalarAsync<int>(sql, entidade, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao adicionar entidade: " + ex.Message, ex);
            }
        }

        public async Task AtualizarEntidade(EntidadeEdicaoDto entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade));

            if (_db.State != ConnectionState.Open)
                _db.Open();
            using var transaction = _db.BeginTransaction();

            try
            {
                const string sql = @"
                UPDATE Entidade SET 
                    TpEntidade = @TpEntidade,
                    RazaoSocial = @RazaoSocial,
                    CNPJ_CPF = @CNPJ_CPF,
                    Telefone = @Telefone,
                    Endereco = @Endereco,
                    Bairro = @Bairro,
                    CEP = @CEP,
                    Cidade = @Cidade,
                    Email = @Email,
                    Sexo = @Sexo,
                    Responsavel = @Responsavel,
                    Ativo = @Ativo
                WHERE Id = @Id";

                await _db.ExecuteAsync(sql, entidade, transaction);

                const string sqlUsuario = @"
                    UPDATE Usuario SET 
                        Email = @Email,
                        Senha = @Senha
                    WHERE EntidadeId = @Id";

                await _db.ExecuteAsync(sqlUsuario, new { Email = entidade.Email, Senha = entidade.Senha, Id = entidade.Id }, transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao atualizar entidade: " + ex.Message, ex);
            }
        }

        public async Task InativarEntidade(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.", nameof(id));

            try
            {
                const string sql = "UPDATE Entidade SET Ativo = 0 WHERE Id = @Id";
                await _db.ExecuteAsync(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inativar entidade: " + ex.Message, ex);
            }
        }

        public async Task<bool> VerificaDocumentoeEmailExistente(string documento, string email)
        {
            try
            {
                var sql = "SELECT COUNT(1) FROM Entidade WHERE CNPJ_CPF = @Documento OR Email = @Email";
                var resultado = await _db.ExecuteScalarAsync<int>(sql, new { Documento = documento, Email = email });
                return resultado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar documento ou email existente: " + ex.Message, ex);
            }
        }
    }
}
