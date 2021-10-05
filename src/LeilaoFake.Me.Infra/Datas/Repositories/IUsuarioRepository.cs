using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioPaginacao> GetAllAsync(UsuarioPaginacao data);
        Task<Usuario> GetByIdAsync(string usuarioId);
        Task<Usuario> GetByEmailAsync(string email);
        Task<string> InsertAsync(Usuario usuario);
        Task DeleteAsync(string usuarioId);
        Task UpdateAsync(Usuario usuario);
    }
}
