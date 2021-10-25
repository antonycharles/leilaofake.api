using Dapper;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public class LanceRepository : ILanceRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IUsuarioRepository _usuarioRepository;

        public LanceRepository(IDbConnection dbConnection, IUsuarioRepository usuarioRepository)
        {
            _dbConnection = dbConnection;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Lance> GetByIdAsync(string lanceId)
        {
            string sql = @"
                SELECT * FROM lances
                WHERE id = @Id";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<Lance>(sql, new { Id = lanceId });

            if(resultado.InteressadoId != null)
                resultado.Interessado = await _usuarioRepository.GetByIdAsync(resultado.InteressadoId);

            return resultado;
        }

        public async Task<IList<Lance>> GetAllByLeilaoIdAsync(string leilaoId)
        {
            string sql = "SELECT * FROM lances WHERE leilaoid = @LeilaoId";

            var resultado = await _dbConnection.QueryAsync<Lance>(sql, new { LeilaoId = leilaoId });

            return resultado.ToList();
        }

        public async Task<string> InsertAsync(Lance lance)
        {
            string sql = @"
                INSERT INTO lances 
                    (leilaoid, interessadoid, criadoem, valor)
                VALUES 
                    (@LeilaoId, @InteressadoId, @CriadoEm, @Valor)
                 RETURNING Id";

            var resultado = await _dbConnection.ExecuteScalarAsync(sql, lance);

            if (resultado == null)
                throw new ArgumentException("Lance não foi cadastrado!");

            return resultado.ToString();
        }
    }
}
