﻿using Dapper;
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
        var sql = "SELECT * FROM Doacao WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Doacao>(sql, new { Id = id });
    }

    public async Task<int> AdicionarDoacao(Doacao doacao)
    {
        var sql = @"INSERT INTO Doacao 
                    (IdOng, IdDoador, DataDoacao, Observacao, Status)
                VALUES (@IdOng, @IdDoador, @DataDoacao, @Observacao, @Status);
                SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await _db.ExecuteScalarAsync<int>(sql, doacao);
        return id;
    }
    public async Task AtualizarDoacao(Doacao doacao)
    {
        var sql = @"UPDATE Doacao
            SET Status = @Status
            WHERE Id = @Id";

        await _db.ExecuteAsync(sql, doacao);
    }

    public async Task CancelarDoacao(int id, string motivoCancelamento)
    {
        var sql = "UPDATE Doacao SET Status = 2, Observacao = @motivoCancelamento WHERE Id = @Id";
        await _db.ExecuteAsync(sql, new { Id = id, motivoCancelamento });
    }


    public async Task<IEnumerable<Doacao>> ObterDoacoesPorDoadorOuOng(FiltroDoacaoDto filtroDoacaoDto)
    {
        if ((filtroDoacaoDto.IdDoador == null || filtroDoacaoDto.IdDoador == 0) &&
            (filtroDoacaoDto.IdOng == null || filtroDoacaoDto.IdOng == 0))
            return Enumerable.Empty<Doacao>();

        var sql = "SELECT * FROM Doacao WHERE YEAR(DataDoacao) >= 2025";

        if (filtroDoacaoDto.IdDoador.HasValue && filtroDoacaoDto.IdDoador.Value != 0)
            sql += " AND IdDoador = @IdDoador";
        if (filtroDoacaoDto.IdOng.HasValue && filtroDoacaoDto.IdOng.Value != 0)
            sql += " AND IdOng = @IdOng";
        if (filtroDoacaoDto.Status.HasValue && filtroDoacaoDto.Status.Value != 0)
            sql += " AND Status = @Status";
        if (filtroDoacaoDto.DataDoacao.HasValue)
            sql += " AND CAST(DataDoacao AS DATE) = @DataDoacao";

        return await _db.QueryAsync<Doacao>(sql, filtroDoacaoDto);
    }


    public async Task<EstatisticasDto> ObterEstatisticas()
    {
        var sql = @"
            SELECT 
                (SELECT COUNT(DISTINCT IdOng) FROM Doacao) AS TotalOngsAjudadas,
                (SELECT SUM(Quantidade) FROM AlimentoDoacao) AS TotalKgAlimentosDoado
        ";

        return await _db.QueryFirstOrDefaultAsync<EstatisticasDto>(sql);
    }

    public async Task<IEnumerable<DoacaoComDetalhes>> ObterDoacoesDoMes(int idOng)
    {
        var dataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var dataFim = dataInicio.AddMonths(1).AddSeconds(-1);

        const string sql = @"
                SELECT d.*, a.Quantidade, a.UnidadeMedida, a.Validade,
                       al.Id AS AlimentoId, al.Nome AS AlimentoNome, e.NomeFantasia AS DoadorNome, o.NomeFantasia AS OngNome
                FROM Doacao d
                    LEFT JOIN AlimentoDoacao a ON a.IdDoacao = d.Id
                    LEFT JOIN Alimento al ON al.Id = a.AlimentoId
                    LEFT JOIN Entidade e ON e.Id = d.IdDoador
                    LEFT JOIN Entidade o ON o.Id = d.IdOng
                WHERE d.IdOng = @IdOng AND d.Status = 3;
        ";

        return await _db.QueryAsync<DoacaoComDetalhes>(sql, new { IdOng = idOng, DataInicio = dataInicio, DataFim = dataFim });
    }
}