using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System.Data;
using TCCDoacaoDeAlimentos.Shared.Models;
using Dapper;
using System.Data.Common;
using System.Text.Json;
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
            try
            {
                Console.WriteLine($"Executando SQL para: {entidade.RazaoSocial}"); // Debug

                var sql = @"
            INSERT INTO CadastroEntidade
            (TipoEntidade, RazaoSocial, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, 
             Email, Sexo, Responsavel, Senha)
            VALUES (@Tipo, @RazaoSocial, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, 
                    @Email, @Sexo, @Responsavel, @Senha)";

                Console.WriteLine($"SQL: {sql}"); // Debug
                Console.WriteLine($"Parâmetros: {JsonSerializer.Serialize(entidade)}"); // Debug

                await _db.ExecuteAsync(sql, entidade);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no repositório: {ex.ToString()}"); // Debug
                throw;
            }
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
            var sql = @"UPDATE CadastroEntidade SET 
        INSERT INTO CadastroEntidade
        (TipoEntidade, RazaoSocial, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, 
         Email, Sexo, Responsavel, Senha, ConfirmarSenha)
        VALUES (@Tipo, @RazaoSocial, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, 
                @Email, @Sexo, @Responsavel, @Senha, @ConfirmarSenha);

         WHERE Id = @Id"";
    ";
            await _db.ExecuteAsync(sql, entidade);
        }

        public async Task DeletarEntidade(int id)
        {
            var sql = "DELETE FROM CadastroEntidade WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Entidade>> BuscarOngsPorCidade(string cidade)
        {
            var sql = "SELECT Id, Nome, Cidade, Latitude, Longitude FROM Ongs WHERE Cidade = @Cidade";

            return await _db.QueryAsync<Entidade>(sql, new { Cidade = cidade });
        }

        public async Task<bool> VerificarCnpjExistente(string documento)
        {
            try
            {
                var sql = "SELECT COUNT(1) FROM CadastroEntidade WHERE CNPJ_CPF = @Documento";
                var resultado = await _db.ExecuteScalarAsync<int>(sql, new { Documento = documento });
                return resultado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar documento: {ex.Message}", ex);
            }
        }
    }
}
