using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System.Data;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;
using Dapper;
namespace BackDoacaoDeAlimentos.Repositorios
{
    public class EntidadeRepositorio : IEntidadeRepositorio
    {
        private readonly IDbConnection _db;

        public EntidadeRepositorio(IDbConnection db)
        {
            _db = db;
        }

        public async Task AdicionarEntidade(Entidade entidade)
        {
            var sql = @"
                INSERT INTO CadastroEntidade
                (TipoEntidade, NomeFantasia, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, Email, Sexo, Responsavel)
                VALUES (@Tipo, @NomeFantasia, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, @Email, @Sexo, @Responsavel);
            ";
            await _db.ExecuteAsync(sql, ong);
        }

        public async Task<Entidade> ObterEntidadePorId(int id)
        {
            var sql = "SELECT * FROM CadastroEntidade WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Entidade>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Entidade>> ObterTodasEntidades()
        {
            var sql = "SELECT * FROM CadastroEntidade";
            return await _db.QueryAsync<Entidade>(sql);
        }

        public async Task AtualizarEntidade(Entidade entidade)
        {
            var sql = @"UPDATE CadastroEntidade SET Nome = @NomeFantasia, Cnpj = @Cnpj, 
               Telefone = @Telefone, Email = @Email, Endereco = @Endereco WHERE Id = @Id";
            await _db.ExecuteAsync(sql, ong);
        }

        public async Task DeletarEntidade(int id)
        {
            var sql = "DELETE FROM CadastroEntidade WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
