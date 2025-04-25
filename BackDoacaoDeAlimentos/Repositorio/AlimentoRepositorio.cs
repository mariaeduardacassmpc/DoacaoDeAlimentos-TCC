using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;

namespace BackDoacaoDeAlimentos.Repositorio
{
    public class AlimentoRepositorio : IAlimentoRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AlimentoRepositorio(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Alimento>> ObterTodosAlimentos()
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Alimentos";
            return await conn.QueryAsync<Alimento>(sql);
        }

        public async Task<Alimento?> ObterAlimentoPorId(int id)
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Alimentos WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Alimento>(sql, new { Id = id });
        }

        public async Task AdicionarAlimento(Alimento alimento)
        {
            using var conn = Connection;
            var sql = @"INSERT INTO Alimentos (Nome, Categoria, Descricao) 
                            VALUES (@Nome, @Categoria, @Descricao)";
            await conn.ExecuteAsync(sql, alimento);
        }

        public async Task RemoverAlimento(int id)
        {
            using var conn = Connection;
            var sql = "DELETE FROM Alimentos WHERE Id = @Id";
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}