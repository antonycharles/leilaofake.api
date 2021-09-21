using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Service.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetByIdAsync(string usuarioId);
        Task<Usuario> GetByEmailAsync(string email);
        Task<UsuarioPaginacao> GetAllAsync(UsuarioPaginacao data);
        Task<Usuario> InsertAsync(Usuario usuario);
        Task UpdateAsync(string usuarioId, Usuario usuario);
        Task DeleteAsync(string usuarioId);
    }
}