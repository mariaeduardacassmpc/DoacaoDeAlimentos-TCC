using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using FrontDoacaoDeAlimentos.Models;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;

public class NotificacaoRepositorio : INotificacaoRepositorio
{
    private readonly string _connectionString;

    public NotificacaoRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Notificacao> AdicionarNotificacao(Notificacao notificacao)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"INSERT INTO Notificacao (IdEntidade, IdDoacao, Mensagem, Observacao, TipoNotificacao)
                          OUTPUT INSERTED.Id
                          VALUES (@IdEntidade, @IdDoacao, @Mensagem, @Observacao, @TipoNotificacao)";

            var id = await connection.ExecuteScalarAsync<int>(query, notificacao);
            notificacao.Id = id;

            return notificacao;
        }
    }
    public async Task<Notificacao> ObterNotificacaoPorId(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Notificacao WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Notificacao>(query, new { Id = id });
        }
    }

    public async Task<IEnumerable<Notificacao>> ObterTodasNotificacoes()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Notificacao";
            return await connection.QueryAsync<Notificacao>(query);
        }
    }
    public async Task DeletarNotificacao(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Notificacao WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
