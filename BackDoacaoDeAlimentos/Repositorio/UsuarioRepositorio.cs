using Dapper;
using System.Data;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System;

namespace BackDoacaoDeAlimentos.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IDbConnection _db;

        public UsuarioRepositorio(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Usuario> ObterPorId(int id)
        {
            const string sql = @"
                SELECT 
                    u.Id, u.EntidadeId, u.Email, u.Senha,
                    e.Id AS Entidade_Id, e.RazaoSocial, e.TpEntidade, e.CNPJ_CPF, e.Telefone, e.Endereco, e.Bairro, e.CEP, e.Cidade, e.Email AS Entidade_Email, e.Responsavel, e.Altitude, e.Latitude
                FROM Usuario u
                INNER JOIN Entidade e ON u.EntidadeId = e.Id
                WHERE u.EntidadeId = @EntidadeId";

            return await _db.QueryFirstOrDefaultAsync<Usuario>(sql, new { EntidadeId = id });
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
             var sql = @"
                SELECT u., e. 
                FROM Usuario u
                INNER JOIN Entidade e ON u.EntidadeId = e.Id
                WHERE u.Email = @Email
             ";

            return await _db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<Usuario> Adicionar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            using var transaction = _db.BeginTransaction();
            try
            {
                var sql = @"
                    INSERT INTO Usuario (EntidadeId, Email, Senha)
                    VALUES (@EntidadeId, @Email, @Senha);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                usuario.Id = await _db.ExecuteScalarAsync<int>(sql, usuario, transaction);
                transaction.Commit();
                return usuario;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task Atualizar(Usuario usuario)
        {
            var sql = @"
                UPDATE Usuario SET
                Email = @Email,
                Senha = @Senha
                WHERE Id = @Id
            ";

         var affectedRows = await _db.ExecuteAsync(sql, usuario);
             
        }

        public async Task<bool> VerificarEmailExistente(string email)
        {
            var sql = "SELECT 1 FROM Usuario WHERE Email = @Email";
            return await _db.ExecuteScalarAsync<bool>(sql, new { Email = email });
        }
    }
}