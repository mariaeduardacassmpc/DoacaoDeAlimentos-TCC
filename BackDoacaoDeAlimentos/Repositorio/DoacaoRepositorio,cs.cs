using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class DoacaoRepository : IDoacaoRepository
{
    private readonly IDbConnection _connection;

    public DoacaoRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Doacao>> ObterTodasAsync()
    {
        var sql = "SELECT * FROM Doacoes";
        return await _connection.QueryAsync<Doacao>(sql);
    }

    public async Task<Doacao> ObterPorIdAsync(int id)
    {
        var sql = "SELECT * FROM Doacoes WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Doacao>(sql, new { Id = id });
    }

    public async Task AdicionarAsync(Doacao doacao)
    {
        var sql = @"
            INSERT INTO Doacoes (NomeAlimento, Quantidade, Data)
            VALUES (@NomeAlimento, @Quantidade, @Data);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await _connection.ExecuteScalarAsync<int>(sql, doacao);
        doacao.DefinirId(id);
    }

    public async Task AtualizarAsync(Doacao doacao)
    {
        var sql = @"
            UPDATE Doacoes
            SET NomeAlimento = @NomeAlimento,
                Quantidade = @Quantidade,
                Data = @Data
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, doacao);
    }

    public async Task DeletarAsync(int id)
    {
        var sql = "DELETE FROM Doacoes WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}
