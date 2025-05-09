using TCCDoacaoDeAlimentos.Shared.Models;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using FrontDoacaoDeAlimentos.Models;
using System.Data;
using Dapper;
using TCCDoacaoDeAlimentos.Models;

namespace BackDoacaoDeAlimentos.Repositorio
{
    public class NotificacaoRepositorio : INotificacaoRepositorio
    {
        private readonly IDbConnection _db;

        public NotificacaoRepositorio(IDbConnection db)
        {
           _db = db;
        }

        //public async Task<Notificacao> AdicionarNotificacao(Notificacao notificacao)
        //{
        //    var sql = @"
        //        INSERT INTO Notificacao
        //        (TipoEntidade, NomeFantasia, CNPJ_CPF, Telefone, Endereco, Bairro, CEP, Cidade, Email, Sexo, Responsavel)
        //        VALUES (@Tipo, @NomeFantasia, @CNPJ_CPF, @Telefone, @Endereco, @Bairro, @CEP, @Cidade, @Email, @Sexo, @Responsavel);
        //    ";
        //    await _db.QueryFirstOrDefaultAsync<Notificacao>(sql, notificacao);
        //}

        public async Task <Notificacao> ObterNotificacaoPorId(int id)
        {
            var sql = "SELECT * FROM CadastroEntidade WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Notificacao>(sql, new { Id = id });
        }

        public Task<IEnumerable<Notificacao>> ObterTodasNotificacoes()
        {
            throw new NotImplementedException();
        }
    }
}
