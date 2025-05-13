using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System.Data;
using TCCDoacaoDeAlimentos.Models;
using TCCDoacaoDeAlimentos.Shared.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrontDoacaoDeAlimentos.Models;

namespace BackDoacaoDeAlimentos.Repositorios
{
    public class NotificacaoRepositorio : INotificacaoRepositorio
    {
        private readonly IDbConnection _db;

        public NotificacaoRepositorio(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Notificacao> ObterNotificacaoPorId(int id)
        {
            var sql = "SELECT * FROM Notificacao WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Notificacao>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Notificacao>> ObterTodasNotificacoes()
        {
            var sql = "SELECT * FROM Notificacao";
            return await _db.QueryAsync<Notificacao>(sql);
        }

        public async Task DeletarNotificacao(int id)
        {
            var sql = "DELETE FROM Notificacao WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Notificacao> AdicionarNotificacao(Notificacao notificacao)
        {
            var sql = @"
                INSERT INTO Notificacao (IdEntidade, IdDoacao, Mensagem, Observacao, TipoNotificacao)
                OUTPUT INSERTED.Id
                VALUES (@IdEntidade, @IdDoacao, @Mensagem, @Observacao, @TipoNotificacao);
            ";

            var id = await _db.ExecuteScalarAsync<int>(sql, notificacao);
            notificacao.Id = id;  

            return notificacao;  
        }
    }
}
