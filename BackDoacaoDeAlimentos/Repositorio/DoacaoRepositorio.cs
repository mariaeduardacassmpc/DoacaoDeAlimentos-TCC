using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Dto;
using TCCDoacaoDeAlimentos.Shared.Models;

public class DoacaoRepositorio : IDoacaoRepositorio
{
    private readonly IDbConnection _db;

    public DoacaoRepositorio(IDbConnection connection)
    {
        _db = connection;
    }

    public async Task<IEnumerable<Doacao>> ObterTodasDoacao()
    {
        var sql = "SELECT * FROM Doacao";
        return await _db.QueryAsync<Doacao>(sql);
    }

    public async Task<Doacao> ObterDoacaoPorId(int id)
    {
        var sql = "SELECT * FROM Doacao WHERE IdDoacao = @Id";
        return await _db.QueryFirstOrDefaultAsync<Doacao>(sql, new { Id = id });
    }

    public async Task AdicionarDoacao(Doacao doacao)
    {
        var sql = @"INSERT INTO Doacao 
                        (NomeAlimento, Quantidade, Data)
                    VALUES (@NomeAlimento, @Quantidade, @Data);
                        SELECT CAST(SCOPE_IDENTITY() as int);
        ";

        var id = await _db.ExecuteScalarAsync<int>(sql, doacao);
    }

    public async Task AtualizarDoacao(Doacao doacao)
    {
        var sql = @"UPDATE Doacao
            SET NomeAlimento = @NomeAlimento,
                Quantidade = @Quantidade,
                Data = @Data
            WHERE Id = @Id";

        await _db.ExecuteAsync(sql, doacao);
    }

    public async Task DeletarDoacao(int id)
    {
        var sql = "DELETE FROM Doacao WHERE IdDoacao = @Id";
        await _db.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<Doacao>> ObterDoacoesPorDoadorOuOng(FiltroDoacaoDto filtroDoacaoDto)
    {
        var sql = "SELECT * FROM Doacao WHERE YEAR(DataDoacao) >= 2025";

        if (filtroDoacaoDto.IdDoador.HasValue && filtroDoacaoDto.IdDoador.Value != 0)
        {
            sql += " AND IdDoador = @IdDoador";
        }
        if (filtroDoacaoDto.IdOng.HasValue && filtroDoacaoDto.IdOng.Value != 0)
        {
            sql += " AND IdOng = @IdOng";
        }
        if (filtroDoacaoDto.Status.HasValue && filtroDoacaoDto.Status.Value != 0)
        {
            sql += " AND Status = @Status";
        }
        //if (filtroDoacaoDto.DataDoacao.HasValue)
        //{
        //    sql += " AND DataDoacao = @DataDoacao";
        //}
        
        return await _db.QueryAsync<Doacao>(sql, filtroDoacaoDto);
    }

    public async Task<EstatisticasDto> ObterEstatisticas()
    {
        var sql = @"
            SELECT 
                (SELECT COUNT(DISTINCT IdOng) FROM Doacoes WHERE Status = 'Entregue') AS TotalOngsAjudadas,
                (SELECT SUM(PesoKg) FROM Doacoes WHERE Status = 'Entregue') AS TotalKgAlimentosDoado
        ";

        return await _db.QueryFirstOrDefaultAsync<EstatisticasDto>(sql);
    }
}
