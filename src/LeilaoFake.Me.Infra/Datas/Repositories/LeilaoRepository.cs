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

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public class LeilaoRepository : ILeilaoRepository
    {
        private readonly IDbConnection _dbConnection;

        public LeilaoRepository(
            IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Leilao> GetByIdAsync(string leilaoId)
        {
            string sql = @"
                SELECT 
                    * 
                FROM leiloes AS LE 
                LEFT JOIN lances AS LA ON LE.id = LA.leilaoid 
                LEFT JOIN usuarios AS U ON LA.interessadoid = U.id
                LEFT JOIN leilaoimagens AS LEI ON LE.id = LEI.leilaoid
                WHERE LE.id = @id";

            var leilaoDictionary = new Dictionary<string, Leilao>();

            var result = await _dbConnection.QueryAsync<Leilao, Lance, Usuario, LeilaoImagem, Leilao>(sql,
                            (leilao, lance, usuario, leilaoImagem) =>
                            {
                                Leilao leilaoEntry;

                                if (!leilaoDictionary.TryGetValue(leilao.Id, out leilaoEntry))
                                {
                                    leilaoEntry = leilao;
                                    leilaoDictionary.Add(leilaoEntry.Id, leilaoEntry);
                                }

                                if (lance != null)
                                {
                                    if (usuario != null)
                                        lance.Interessado = usuario;

                                    leilaoEntry.Lances.Add(lance);
                                }

                                if (leilaoImagem != null)
                                    leilaoEntry.LeilaoImagems.Add(leilaoImagem);

                                return leilaoEntry;
                            },
                            new { id = leilaoId });

            return result.FirstOrDefault();
        }

        public async Task<Leilao> GetByIdAndLeiloadoPorIdAsync(string leilaoId, string leiloadoPorId)
        {
            string sql = @"
                SELECT 
                    * 
                FROM leiloes AS LE 
                LEFT JOIN lances AS LA ON LE.id = LA.leilaoid 
                LEFT JOIN leilaoimagens AS LEI ON LE.id = LEI.leilaoid
                WHERE LE.id = @id AND LE.leiloadoporid = @leiloadoPorId";

            var leilaoDictionary = new Dictionary<string, Leilao>();

            var result = await _dbConnection.QueryAsync<Leilao, Lance, LeilaoImagem, Leilao>(sql,
                            (leilao, lance, leilaoImagem) =>
                            {
                                Leilao leilaoEntry;

                                if (!leilaoDictionary.TryGetValue(leilao.Id, out leilaoEntry))
                                {
                                    leilaoEntry = leilao;
                                    leilaoDictionary.Add(leilaoEntry.Id, leilaoEntry);
                                }

                                if (lance != null)
                                    leilaoEntry.Lances.Add(lance);

                                if (leilaoImagem != null)
                                    leilaoEntry.LeilaoImagems.Add(leilaoImagem);

                                return leilaoEntry;
                            },
                            new { id = leilaoId, leiloadoPorId = leiloadoPorId });

            return result.FirstOrDefault();
        }

        public async Task<LeilaoPaginacao> GetAllAsync(LeilaoPaginacao data)
        {
            string sql = String.Format(@"
                SELECT
                    COUNT(id)
                FROM leiloes
                WHERE 
                    (status = @Status OR @Status IS NULL) AND
                    (titulo ILIKE @SearchDb OR @SearchDb IS NULL) AND
                    ((datainicio >= now() OR datafim >= now()) OR @LeiloadoPorId IS NOT NULL) AND
                    (ispublico = @IsPublico OR @LeiloadoPorId IS NOT NULL) AND
                    (leiloadoporid = @LeiloadoPorId OR @LeiloadoPorId IS NULL);
                SELECT 
                    LE.*,
                    (SELECT COUNT(id) FROM lances WHERE leilaoid = LE.id) AS totallances,
	                (SELECT url FROM leilaoimagens WHERE leilaoid = LE.id LIMIT 1) AS caminhoimagem
                FROM leiloes AS LE 
                WHERE 
                    (LE.status = @Status OR @Status IS NULL) AND
                    (LE.titulo ILIKE @SearchDb OR @SearchDb IS NULL) AND
                    ((datainicio >= now() OR datafim >= now()) OR @LeiloadoPorId IS NOT NULL) AND
                    (ispublico = @IsPublico OR @LeiloadoPorId IS NOT NULL) AND
                    (leiloadoporid = @LeiloadoPorId OR @LeiloadoPorId IS NULL)
                ORDER BY {0}
                LIMIT @PorPagina 
                OFFSET(@Pagina - 1) * @PorPagina;
            ", data.Order);

            using (var result = await _dbConnection.QueryMultipleAsync(sql, data))
            {
                data.Total = result.Read<int>().FirstOrDefault();
                data.Resultados = result.Read<Leilao>().ToList();
            }

            return data;
        }

        public async Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId)
        {
            string sql = @"
                SELECT 
                    * 
                FROM leiloes AS LE 
                LEFT JOIN lances AS LA ON LE.id = LA.leilaoid
                LEFT JOIN leilaoimagens AS LEI ON LE.id = LEI.leilaoid
                WHERE LE.leiloadoPorId = @leiloadoPorId";

            var leilaoDictionary = new Dictionary<string, Leilao>();
            var result = await _dbConnection.QueryAsync<Leilao, Lance, LeilaoImagem, Leilao>(sql,
                            (leilao, lance, leilaoImagem) =>
                            {
                                Leilao leilaoEntry;

                                if (!leilaoDictionary.TryGetValue(leilao.Id, out leilaoEntry))
                                {
                                    leilaoEntry = leilao;
                                    leilaoDictionary.Add(leilaoEntry.Id, leilaoEntry);
                                }

                                if (lance != null)
                                    leilaoEntry.Lances.Add(lance);

                                if (leilaoImagem != null)
                                    leilaoEntry.LeilaoImagems.Add(leilaoImagem);

                                return leilaoEntry;
                            },
                            new { leiloadoPorId = leiloadoPorId });

            return result.ToList();
        }

        public async Task<string> InsertAsync(Leilao leilao)
        {
            string sql = @"
                INSERT INTO leiloes 
                    (leiloadoporid, titulo, descricao, lanceminimo, datainicio, datafim, status, lanceganhadorid, ispublico, criadoem)
                VALUES
                    (@LeiloadoPorId, @Titulo, @Descricao, @LanceMinimo, @DataInicio, @DataFim, @status, @LanceGanhadorId, @IsPublico, @CriadoEm)
                RETURNING Id";

            var leilaoId = await _dbConnection.ExecuteScalarAsync(sql, leilao);

            if (leilaoId == null)
                throw new Exception("Leilão não foi criado!");

            return leilaoId.ToString();
        }

        public async Task UpdateAsync(Leilao leilao)
        {
            string sql = @"
                UPDATE leiloes SET 
                    leiloadoporid = @LeiloadoPorId, 
                    titulo = @Titulo, 
                    descricao = @Descricao, 
                    lanceminimo = @LanceMinimo, 
                    datainicio = @DataInicio,
                    datafim = @DataFim, 
                    status = @Status, 
                    lanceganhadorid = @LanceGanhadorId,
                    ispublico = @IsPublico,
                    alteradoem  = @AlteradoEm
                WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, leilao);

            if (resultado == 0)
                throw new Exception("Leilão não foi alterado!");
        }

        public async Task DeleteAsync(string leilaoId)
        {
            string sql = @"
                DELETE FROM leilaoimagens WHERE leilaoid = @LeilaoId;
                DELETE FROM leiloes WHERE Id = @LeilaoId;";

            var resultado = await _dbConnection.ExecuteAsync(sql, new { LeilaoId = leilaoId });

            if (resultado == 0)
                throw new Exception("Leilão não foi deletado");

        }
    }
}
