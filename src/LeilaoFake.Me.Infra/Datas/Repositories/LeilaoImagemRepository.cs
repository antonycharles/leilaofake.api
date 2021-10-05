using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public class LeilaoImagemRepository : ILeilaoImagemRepository
    {
        private readonly IDbConnection _dbConnection;

        public LeilaoImagemRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> InsertAsync(LeilaoImagem leilaoImagem)
        {
            string sql = @"
                INSERT INTO leilaoimagens 
                    (leilaoid, url)
                VALUES 
                    (@LeilaoId, @Url)
                 RETURNING Id";

            var resultado = await _dbConnection.ExecuteScalarAsync(sql, leilaoImagem);

            if (resultado == null)
                throw new ArgumentException("Leilão imagem não foi cadastrado!");

            return Int32.Parse(resultado.ToString());
        }

        public async Task DeleteAsync(int leilaoImagemId)
        {
            string sql = @"
                DELETE FROM leilaoimagens 
                WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, new{ Id = leilaoImagemId });

            if (resultado == 0)
                throw new Exception("Leilão imagem não foi deletado");
        }

        public async Task<LeilaoImagem> GetByIdAsync(int leilaoImagemId)
        {
            string sql = "SELECT * FROM leilaoimagens WHERE id = @Id";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<LeilaoImagem>(sql, new { Id = leilaoImagemId });

            return resultado;
        }
    }
}