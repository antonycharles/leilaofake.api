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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsuarioRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task DeleteAsync(string usuarioId)
        {
            string sql = $"DELETE FROM usuarios WHERE Id = @UsuarioId";

            var resultado = await _dbConnection.ExecuteAsync(sql, new{ UsuarioId = usuarioId });

            if (resultado == 0)
                throw new Exception("Usuário não foi deletado");

        }

        public async Task<UsuarioPaginacao> GetAllAsync(UsuarioPaginacao data)
        {
            string sql = String.Format(@"
                SELECT count(id) FROM usuarios;
                SELECT * FROM usuarios ORDER BY {0} LIMIT @PorPagina OFFSET(@Pagina - 1) * @PorPagina;
            ",data.Order);

            using (var result = await _dbConnection.QueryMultipleAsync(sql, data))
            {
                data.Total = result.Read<int>().FirstOrDefault();
                data.Resultados = result.Read<Usuario>().ToList();
            }

            return data;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            string sql = $"SELECT * FROM usuarios WHERE email = @Email";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });

            return resultado;
        }

        public async Task<Usuario> GetByIdAsync(string usuarioId)
        {
            string sql = $"SELECT * FROM usuarios WHERE Id = @Id";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = usuarioId });

            return resultado;
        }

        public async Task<Usuario> InsertAsync(Usuario usuario) 
        {
            string sql = $"INSERT INTO usuarios ( nome, email, criadoem ) VALUES (@Nome, @Email, @CriadoEm)  RETURNING Id";

            var resultado = await _dbConnection.ExecuteScalarAsync(sql, usuario);

            if (resultado == null)
                throw new Exception("Usuário não foi criado");

            return await this.GetByIdAsync(resultado.ToString());
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            string sql = $"UPDATE usuarios SET nome = @Nome, email = @Email, alteradoem  = @AlteradoEm WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, usuario);

            if (resultado == 0)
                throw new Exception("Usuário não foi alterado");
        }
    }
}
