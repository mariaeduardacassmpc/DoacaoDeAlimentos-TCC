using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;

namespace BackDoacaoDeAlimentos.Repositorios
{
    public class AlimentoDoacaoRepositorio : IAlimentoDoacaoRepositorio
    {
        private readonly IDbConnection _conexao;

        public AlimentoDoacaoRepositorio(IDbConnection conexao)
        {
            _conexao = conexao;
        }

        public async Task<AlimentoDoacao> AdicionarAsync(AlimentoDoacao alimentoDoacao)
        {
            var sql = @"
                INSERT INTO AlimentoDoacao (IdDoacao, AlimentoId, Validade, Quantidade, UnidadeMedida)
                VALUES (@IdDoacao, @AlimentoId, @Validade, @Quantidade, @UnidadeMedida);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var id = await _conexao.ExecuteScalarAsync<int>(sql, alimentoDoacao);
            alimentoDoacao.Id = id;
            return alimentoDoacao;
        }

        public async Task AtualizarAsync(AlimentoDoacao alimentoDoacao)
        {
            var sql = @"
                UPDATE AlimentoDoacao
                SET IdDoacao = @IdDoacao,
                    AlimentoId = @AlimentoId,
                    Validade = @Validade,
                    Quantidade = @Quantidade,
                    UnidadeMedida = @UnidadeMedida
                WHERE Id = @Id;
            ";

            await _conexao.ExecuteAsync(sql, alimentoDoacao);
        }

        public async Task RemoverAsync(int id)
        {
            var sql = "DELETE FROM AlimentoDoacao WHERE Id = @Id;";
            await _conexao.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<AlimentoDoacao> ObterPorIdAsync(int id)
        {
            var sql = "SELECT * FROM AlimentoDoacao WHERE Id = @Id;";
            return await _conexao.QueryFirstOrDefaultAsync<AlimentoDoacao>(sql, new { Id = id });
        }

        public async Task<IEnumerable<AlimentoDoacao>> ObterTodosAsync()
        {
            var sql = "SELECT * FROM AlimentoDoacao;";
            return await _conexao.QueryAsync<AlimentoDoacao>(sql);
        }

        public async Task<IEnumerable<AlimentoDoacao>> ObterPorDoacaoAsync(int doacaoId)
        {
            var sql = "SELECT * FROM AlimentoDoacao WHERE IdDoacao = @DoacaoId;";
            return await _conexao.QueryAsync<AlimentoDoacao>(sql, new { DoacaoId = doacaoId });
        }
    }
}
