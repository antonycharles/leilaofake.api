using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Core.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetByIdAsync(string usuarioId);
        Task<Usuario> GetByEmailAsync(string email);
        Task<IList<Usuario>> GetAllAsync(UsuarioPaginacao data);
        Task<Usuario> InsertAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(string usuarioId);
    }
}