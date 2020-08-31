using System;
using Dapper;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using System.Data;
using System.Linq;
using LeilaoFake.Me.Core.Enums;
using System.Runtime.InteropServices;
using Dapper.Contrib.Extensions;

namespace LeilaoFake.Me.Core.Repositories
{
    public class LeilaoRepository : ILeilaoRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IUsuarioRepository _usuarioRepository;

        public LeilaoRepository(
            IDbConnection dbConnection,
            IUsuarioRepository usuarioRepository)
        {
            _dbConnection = dbConnection;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Leilao> GetLeilaoByIdAsync(string leilaoId)
        {
            string sql = "SELECT * FROM leiloes AS LE LEFT JOIN lances AS LA ON LE.Id = LA.LeilaoId WHERE LE.id = @id";
            var result = await _dbConnection.QueryAsync<Leilao, Lance, Leilao>(sql,
                            (leilao, lance) =>
                            {
                                Leilao leilaoEntry = leilao;

                                leilaoEntry.Lances.Add(lance);

                                return leilaoEntry;
                            },
                            new { id = leilaoId });

            return result.FirstOrDefault();
        }

        public async Task<IList<Leilao>> GetLeiloesAllByEmAndamentoAsync()
        {
            string sql = "SELECT * FROM leiloes AS LE LEFT JOIN lances AS LA ON LE.Id = LA.LeilaoId WHERE LE.status = @status";
            var result = await _dbConnection.QueryAsync<Leilao, Lance, Leilao>(sql,
                            (leilao, lance) =>
                            {
                                Leilao leilaoEntry = leilao;

                                leilaoEntry.Lances.Add(lance);

                                return leilaoEntry;
                            },
                            new { status = StatusLeilaoEnum.EmAndamento });

            return result.ToList();
        }

        public async Task<IList<Leilao>> GetLeiloesAllByLeiloadoPorIdAsync(string leiloadoPorId)
        {
            string sql = "SELECT * FROM leiloes AS LE LEFT JOIN lances AS LA ON LE.Id = LA.LeilaoId WHERE LE.LeiloadoPorId = @leiloadoPorId";
            var result = await _dbConnection.QueryAsync<Leilao, Lance, Leilao>(sql,
                            (leilao, lance) =>
                            {
                                Leilao leilaoEntry = leilao;

                                leilaoEntry.Lances.Add(lance);

                                return leilaoEntry;
                            },
                            new { leiloadoPorId = leiloadoPorId });

            return result.ToList();
        }

        public async Task<Leilao> InsertLeilaoAsync(Leilao leilao)
        {
            var usuario = await _usuarioRepository.InsertUsuarioAsync(leilao.LeiloadoPor);

            if (usuario.Id != leilao.LeiloadoPor.Id)
                throw new ArgumentException("Usuário informado é inválido!");

            string sql = "INSERT INTO leiloes (Id, LeiloadoPorId, Titulo, Descricao, LanceMinimo, DataInicio, DataFim, status, LanceGanhadorId)" +
                        "VALUES(@Id, @LeiloadoPorId, @Titulo, @Descricao, @LanceMinimo, @DataInicio, @DataFim, @status, @LanceGanhadorId)";

            var resultado = await _dbConnection.ExecuteAsync(sql, leilao);

            if (resultado == 0)
                throw new ArgumentException("Leilão não foi criado!");

            return leilao;
        }

        public async Task UpdateCancelarLeilaoAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAsync(leilaoId);

            if (leilao.Id == null || leilao.LeiloadoPorId != leiloadoPorId)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.CancelarLeilao();

            string sql = "UPDATE leiloes SET Status = @Status WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, leilao);

            if (resultado == 0)
                throw new ArgumentException("Leilão não foi alterado!");
        }

        public async Task UpdateFinalizarLeilaoAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAsync(leilaoId);

            if (leilao.Id == null || leilao.LeiloadoPorId != leiloadoPorId)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.FinalizarLeilao();

            string sql = "UPDATE leiloes SET Status = @Status, LanceGanhadorId = @LanceGanhadorId WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, leilao);

            if (resultado == 0)
                throw new ArgumentException("Leilão não foi alterado!");
        }
    }
}
