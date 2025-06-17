using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using TCCDoacaoDeAlimentos.Shared.Dto;
using System.Security.Cryptography;

namespace BackDoacaoDeAlimentos.Data.Repositorios
{
    public class RelatorioRepositorio : IRelatorioRepositorio
    {
        private readonly IDbConnection _dbConnection;

        public RelatorioRepositorio(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> ObterTotalDoacoesMes(int ongId)
        {
            var sql = @"
               SELECT COUNT(DISTINCT Id) 
                    FROM Doacao 
                    WHERE IdOng = @OngId
                      AND MONTH(DataDoacao) = MONTH(GETDATE()) 
                      AND YEAR(DataDoacao) = YEAR(GETDATE())
            ";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new { OngId = ongId });
        }

        public async Task<int> ObterTotalUsuarios()
        {
            var sql = @"SELECT COUNT(*) FROM Usuario";

            return await _dbConnection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<IEnumerable<AlimentoPorCategoriaDto>> ObterAlimentosPorCategoria(int ongId)
        {
            var sql = @"
                    SELECT a.Categoria AS Categoria, SUM(ad.Quantidade) AS TotalKg
                        FROM AlimentoDoacao ad
                        INNER JOIN Alimento a ON ad.AlimentoId = a.Id
                        INNER JOIN Doacao d ON ad.IdDoacao = d.Id
                        WHERE d.IdOng = @OngId
                    GROUP BY a.Categoria";

            return await _dbConnection.QueryAsync<AlimentoPorCategoriaDto>(sql, new { OngId = ongId });
        }

        public async Task<double> ObterTotalKgAlimentos(int ongId)
        {
            var sql = @"
                    SELECT ISNULL(SUM(ad.Quantidade), 0) AS TotalKg
                        FROM AlimentoDoacao ad
                        INNER JOIN Doacao d ON ad.IdDoacao = d.Id
                    WHERE d.IdOng = @OngId";

            return await _dbConnection.ExecuteScalarAsync<double>(sql, new { OngId = ongId });
        }
    }
}
