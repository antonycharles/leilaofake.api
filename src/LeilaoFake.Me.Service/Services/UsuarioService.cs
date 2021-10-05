using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Datas.Repositories;

namespace LeilaoFake.Me.Service.Services
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

        public async Task<UsuarioPaginacao> GetAllAsync(UsuarioPaginacao data)
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
            var usuarioDb = await this.GetByEmailAsync(usuario.Email);

            if(usuarioDb != null)
                return usuarioDb;

            string usuarioId = await _usuarioRepository.InsertAsync(usuario);

            return await _usuarioRepository.GetByIdAsync(usuarioId);
        }

        public async Task UpdateAsync(string usuarioId, Usuario usuario)
        {
            var usuarioDb = await _usuarioRepository.GetByIdAsync(usuarioId);

            if (usuarioDb.Id == null)
                throw new Exception("Usuário não encontrado");

            usuarioDb.Update(usuario);

            await _usuarioRepository.UpdateAsync(usuarioDb);
        }
    }
}