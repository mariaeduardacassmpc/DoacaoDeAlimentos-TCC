using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using Dapper;
using System.Data;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;

namespace BackDoacaoDeAlimentos.Repositorios
{
    public class EntidadeRepositorio : IEntidadeRepositorio
    {
        private readonly IDbConnection _connection;

        public EntidadeRepositorio(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AdicionarOng(Entidade ong)
        {
            var sql = @"
                INSERT INTO CadastroEntidade
                (TipoEntidade, NomeFantasia, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, Email, Sexo, Responsavel)
                VALUES (@Tipo, @NomeFantasia, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, @Email, @Sexo, @Responsavel);
            "; 
            await _connection.ExecuteAsync(sql, ong);
        }

        public async Task<Entidade> ObterOngPorId(int id)
        {
            var sql = "SELECT * FROM CadastroEntidade WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Entidade>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Entidade>> ObterTodasOngs()
        {
            var sql = "SELECT * FROM CadastroEntidade";
            return await _connection.QueryAsync<Entidade>(sql);
        }

        public async Task AtualizarOng(Entidade ong)
        {
            var sql = "UPDATE CadastroEntidade SET Nome = @Nome, Cnpj = @Cnpj, Telefone = @Telefone, Email = @Email, Endereco = @Endereco WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, ong);
        }

        public async Task DeletarOng(int id)
        {
            var sql = "DELETE FROM CadastroEntidade WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
