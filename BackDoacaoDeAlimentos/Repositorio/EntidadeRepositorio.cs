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

        public async Task<IEnumerable<Entidade>> ObterTodasInstituicoes()
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

        public async Task<int> AdicionarEntidade(Entidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException(nameof(entidade));

            entidade.Id = 0;

            if (_db.State != ConnectionState.Open)
                _db.Open();
            using var transaction = _db.BeginTransaction();

            try
            {
                const string sql = @"
                    INSERT INTO Entidade (
                        TpEntidade, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, Email, Sexo, Responsavel, Latitude, Longitude, NomeFantasia
                    ) VALUES (
                        @TpEntidade, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, @Email, @Sexo, @Responsavel, @Latitude, @Longitude, @NomeFantasia
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                ";

                var parametros = new
                {
                    TpEntidade = entidade.TpEntidade.ToString(),
                    CNPJ_CPF = entidade.CNPJ_CPF,
                    Telefone = entidade.Telefone,
                    Endereco = entidade.Endereco,
                    Bairro = entidade.Bairro,
                    CEP = entidade.CEP,
                    Cidade = entidade.Cidade,
                    Email = entidade.Email,
                    Sexo = entidade.Sexo,
                    Responsavel = entidade.Responsavel,
                    Latitude = entidade.Latitude,
                    Longitude = entidade.Longitude,
                    NomeFantasia = entidade.NomeFantasia
                };

                var entidadeId = await _db.ExecuteScalarAsync<int>(sql, parametros, transaction);
                transaction.Commit();
                return entidadeId;
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
                    CNPJ_CPF = @CNPJ_CPF,
                    Telefone = @Telefone,
                    Endereco = @Endereco,
                    Bairro = @Bairro,
                    CEP = @CEP,
                    Cidade = @Cidade,
                    Email = @Email,
                    Sexo = @Sexo,
                    Responsavel = @Responsavel
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

        public async Task<IEnumerable<Entidade>> ObterOngsProximas(double latitude, double longitude, double raioKm = 20)
        {
            try
            {
                var sql = @"
                    SELECT *, 
                        (6371 * acos(
                            cos(radians(@Latitude)) * cos(radians(Latitude)) *
                            cos(radians(Altitude) - radians(@Longitude)) +
                            sin(radians(@Latitude)) * sin(radians(Latitude))
                        )) AS Distancia
                    FROM Entidade
                    WHERE TpEntidade = 'O'
                    AND Latitude IS NOT NULL AND Altitude IS NOT NULL
                    HAVING Distancia <= @RaioKm
                    ORDER BY Distancia
                ";

                return await _db.QueryAsync<Entidade>(sql, new { Latitude = latitude, Longitude = longitude, RaioKm = raioKm });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter ONGs próximas: " + ex.Message, ex);
            }
        }
    }
}
