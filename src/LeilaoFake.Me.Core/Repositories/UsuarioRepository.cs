using Dapper;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Core.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsuarioRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Usuario> GetUsuariobyId(string usuarioId)
        {
            string sql = "SELECT * FROM usuarios WHERE Id = @Id";

            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = usuarioId });

            return resultado;
        }

        public async Task<Usuario> InsertUsuarioAsync(Usuario usuario)
        {
            var usuarioExiste = await this.GetUsuariobyId(usuario.Id);

            if (usuarioExiste != null)
                return usuarioExiste;
            
            string sql = "INSERT INTO usuarios (Id, Nome ) VALUES (@Id, @Nome)";

            var resultado = await _dbConnection.ExecuteAsync(sql, usuario);

            if (resultado == 0)
                throw new ArgumentException("Usuário não foi criado!");

            return usuario;
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            var usuarioDb = await this.GetUsuariobyId(usuario.Id);

            if (usuarioDb.Id == null)
                throw new ArgumentException("Usuário não encontrado!");

            usuarioDb.Update(usuario);

            string sql = "UPDATE usuarios SET Nome = @Nome WHERE Id = @Id";

            var resultado = await _dbConnection.ExecuteAsync(sql, usuarioDb);

            if (resultado == 0)
                throw new ArgumentException("Usuário não foi alterado!");
        }
    }
}
