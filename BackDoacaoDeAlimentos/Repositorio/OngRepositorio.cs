using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using Dapper;
using System.Data;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Repositorios
{
    public class OngRepositorio : IOngRepositorio
    {
        private readonly IDbConnection _connection;

        public OngRepositorio(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AdicionarOng(Entidade ong)
        {
            var sql = "INSERT INTO Ongs (Nome, Cnpj, Telefone, Email, Endereco) VALUES (@Nome, @Cnpj, @Telefone, @Email, @Endereco)";
            await _connection.ExecuteAsync(sql, ong);
        }

        public async Task<Entidade> ObterOngPorId(int id)
        {
            var sql = "SELECT * FROM Ongs WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Entidade>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            var sql = "SELECT * FROM Ongs";
            return await _connection.QueryAsync<Entidade>(sql);
        }

        public async Task AtualizarOng(Entidade ong)
        {
            var sql = "UPDATE Ongs SET Nome = @Nome, Cnpj = @Cnpj, Telefone = @Telefone, Email = @Email, Endereco = @Endereco WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, ong);
        }

        public async Task DeletarOng(int id)
        {
            var sql = "DELETE FROM Ongs WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
