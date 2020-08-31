using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Core.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetUsuariobyId(string usuarioId);
        Task<Usuario> InsertUsuarioAsync(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
    }
}
