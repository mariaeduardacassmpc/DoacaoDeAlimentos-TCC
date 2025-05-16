using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System.Data.Common;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Repositorio
{
    public class AlimentoRepositorio : IAlimentoRepositorio
    {
        private readonly IDbConnection _db;

        public AlimentoRepositorio(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Alimento>> ObterTodosAlimentos()
        {
            var sql = @"SELECT * FROM Alimento";
            return await _db.QueryAsync<Alimento>(sql);
        }

        public async Task<Alimento> ObterAlimentoPorId(int id)
        {
            var sql = "SELECT * FROM Alimento WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Alimento>(sql, new { Id = id });
        }

        public async Task AdicionarAlimento(Alimento alimento)
        {
            var sql = @"INSERT INTO Alimento
                            (Nome, Categoria, Descricao) 
                               OUTPUT INSERTED.* VALUES 
                            (@Nome, @Categoria, @Descricao); 
            ";

            await _db.QuerySingleAsync<Alimento>(sql, alimento);
        }

        public async Task DeletarAlimento(int id)
        {
            var sql = @"DELETE FROM Alimento
                             WHERE Id = @Id    
            ";

            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task AtualizarAlimentos(Alimento alimento)
        {
            var sql = @"UPDATE Alimento
                        SET Nome = @Nome, 
                           Categoria = @Categoria, 
                           Descricao = @Descricao
                        WHERE Id = @Id;
            ";

            await _db.ExecuteAsync(sql, alimento);
        }
    }
}