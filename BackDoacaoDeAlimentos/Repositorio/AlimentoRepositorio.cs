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
            var sql = @"SELECT * FROM Alimento WHERE Ativo = 1";
            return await _db.QueryAsync<Alimento>(sql);
        }

        public async Task<Alimento> ObterAlimentoPorId(int id)
        {
            var sql = "SELECT * FROM Alimento WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Alimento>(sql, new { Id = id });
        }

        public async Task AdicionarAlimento(Alimento alimento)
        {
            var sql = @"INSERT INTO Alimento (Nome, Categoria)
                VALUES (@Nome, @Categoria)";
            await _db.ExecuteAsync(sql, alimento);
        }

        public async Task InativarAlimento(int id)
        {
            var sql = @"UPDATE Alimento SET Ativo = 0 WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }


        public async Task AtualizarAlimentos(Alimento alimento)
        {
            var sql = @"UPDATE Alimento
                        SET Nome = @Nome, 
                           Categoria = @Categoria
                        WHERE Id = @Id;
            ";

            await _db.ExecuteAsync(sql, alimento);
        }
    }
}