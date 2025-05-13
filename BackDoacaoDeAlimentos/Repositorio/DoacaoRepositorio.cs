using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;

public class DoacaoRepositorio : IDoacaoRepositorio
{
    private readonly IDbConnection _connection;

    public DoacaoRepositorio(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Doacao>> ObterTodasDoacao()
    {
        var sql = "SELECT * FROM Doacao";
        return await _connection.QueryAsync<Doacao>(sql);
    }

    public async Task<Doacao> ObterDoacaoPorId(int id)
    {
        var sql = "SELECT * FROM Doacao WHERE IdDoacao = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Doacao>(sql, new { Id = id });
    }

    public async Task AdicionarDoacao(Doacao doacao)
    {
        var sql = @"INSERT INTO Doacao 
                        (NomeAlimento, Quantidade, Data)
                    VALUES (@NomeAlimento, @Quantidade, @Data);
                        SELECT CAST(SCOPE_IDENTITY() as int);
        ";

        var id = await _connection.ExecuteScalarAsync<int>(sql, doacao);
    }

    public async Task AtualizarDoacao(Doacao doacao)
    {
        var sql = @"UPDATE Doacao
            SET NomeAlimento = @NomeAlimento,
                Quantidade = @Quantidade,
                Data = @Data
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, doacao);
    }

    public async Task DeletarDoacao(int id)
    {
        var sql = "DELETE FROM Doacao WHERE IdDoacao = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}
