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
                SELECT u., e. 
                FROM Usuarios u
                INNER JOIN Entidades e ON u.EntidadeId = e.Id
                WHERE u.Id = @Id";

            return await _db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            const string sql = @"
                SELECT u., e. 
                FROM Usuarios u
                INNER JOIN Entidades e ON u.EntidadeId = e.Id
                WHERE u.Email = @Email";

            return await _db.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<Usuario> Adicionar(Usuario usuario)
        {
            const string sql = @"
                INSERT INTO Usuario 
                (EntidadeId, Email, Senha)
                OUTPUT INSERTED.*
                VALUES 
                (@EntidadeId, @Email, @Senha)";

            return await _db.QuerySingleAsync<Usuario>(sql, usuario);
        }

        public async Task Atualizar(Usuario usuario)
        {
            const string sql = @"
                UPDATE Usuarios SET
                    Email = @Email,
                    Senha = @Senha
                WHERE Id = @Id";

            var affectedRows = await _db.ExecuteAsync(sql, usuario);
            if (affectedRows == 0)
            {
                throw new ArgumentException("Usuário não encontrado para atualização");
            }
        }

        public async Task Remover(int id)
        {
            const string sql = "DELETE FROM Usuarios WHERE Id = @Id";

            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id });
            if (affectedRows == 0)
            {
                throw new ArgumentException("Usuário não encontrado para remoção");
            }
        }

        public async Task<bool> VerificarEmailExistente(string email)
        {
            const string sql = "SELECT 1 FROM Usuarios WHERE Email = @Email";
            return await _db.ExecuteScalarAsync<bool>(sql, new { Email = email });
        }
    }
}