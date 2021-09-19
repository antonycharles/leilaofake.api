using Dapper;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Infra.Data.Repositories
{
    public class LanceRepository : ILanceRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILeilaoRepository _leilaoRepository;

        public LanceRepository(IDbConnection dbConnection,
            IUsuarioRepository usuarioRepository,
            ILeilaoRepository leilaoRepository)
        {
            _dbConnection = dbConnection;
            _usuarioRepository = usuarioRepository;
            _leilaoRepository = leilaoRepository;
        }

        public async Task<Lance> GetById(string lanceId)
        {
            string sql = "SELECT * FROM lances WHERE Id = @Id";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<Lance>(sql, new { Id = lanceId });

            return resultado;
        }

        public async Task<IList<Lance>> GetAllByLeilaoId(string leilaoId)
        {
            string sql = "SELECT * FROM lances WHERE LeilaoId = @LeilaoId";

            var resultado = await _dbConnection.QueryAsync<Lance>(sql, new { LeilaoId = leilaoId });

            return resultado.ToList();
        }

        public async Task<Lance> InsertAsync(Lance lance)
        {

            var leilao = await _leilaoRepository.GetByIdAsync(lance.LeilaoId);

            if (leilao == null)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.RecebeLance(lance);

            var usuario = await _usuarioRepository.InsertAsync(lance.Interessado);

            if (usuario.Id != lance.Interessado.Id)
                throw new ArgumentException("Usuário informado é inválido!");

            string sql = "INSERT INTO lances (Id, LeilaoId, InteressadoId, Data, Valor ) " +
                            "VALUES (@Id, @LeilaoId, @InteressadoId, @Data, @Valor)";

            var resultado = await _dbConnection.ExecuteAsync(sql, lance);

            if (resultado == 0)
                throw new ArgumentException("Lance não foi cadastrado!");

            return lance;
        }
    }
}
