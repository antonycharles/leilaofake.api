using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;

namespace LeilaoFake.Me.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task DeleteAsync(string usuarioId)
        {
            await _usuarioRepository.DeleteAsync(usuarioId);
        }

        public async Task<IList<Usuario>> GetAllAsync(UsuarioPaginacao data)
        {
            return await _usuarioRepository.GetAllAsync(data);
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<Usuario> GetByIdAsync(string usuarioId)
        {
            return await _usuarioRepository.GetByIdAsync(usuarioId);
        }

        public async Task<Usuario> InsertAsync(Usuario usuario)
        {
            return await _usuarioRepository.InsertAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            var usuarioDb = await _usuarioRepository.GetByIdAsync(usuario.Id);

            if (usuarioDb.Id == null)
                throw new ArgumentException("Usuário não encontrado");

            usuarioDb.Update(usuario);
        }
    }
}