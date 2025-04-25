using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TCCDoacaoDeAlimentos.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using System.Data.Common;

namespace BackDoacaoDeAlimentos.Repositorio
{
    public class AlimentoRepositorio : IAlimentoRepositorio
    {
        private readonly IDbConnection _db;

        public AlimentoRepositorio(IDbConnection db)
        {
            _db = db;
        }
        public IEnumerable<Alimento> ObterTodosAlimentos()
        {
            var sql = "SELECT * FROM Alimentos";
            return _db.Query<Alimento>(sql);
        }

        public Alimento ObterAlimentoPorId(int id)
        {
            var sql = "SELECT * FROM Alimentos WHERE Id = @Id";
            return _db.QueryFirstOrDefault<Alimento>(sql, new { Id = id });
        }

        public Alimento GravarAlimento(Alimento alimento)
        {
            var sql = @"INSERT INTO Alimentos (Nome, Categoria, Descricao) 
                    OUTPUT INSERTED.* VALUES (@Nome, @Categoria, @Descricao); 
            ";
            return _db.QuerySingle<Alimento>(sql, alimento);
        }

        public void RemoverAlimento(int id)
        {
            var sql = "DELETE FROM Alimentos WHERE Id = @Id";
            _db.ExecuteAsync(sql, new { Id = id });
        }
    }
}