using LeilaoFake.Me.Core.Exceptions;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LeilaoFake.Me.Test.Services
{
    
    public class UsuarioServiceTest
    {
        [Fact]
        public async Task IncluirNovoUsuarioComSucessoAsync()
        {
            var usuarioService = GetUsuarioService();

            var usuario = await usuarioService.InsertAsync(new Usuario("Antony Charles", "antony.charles@gmail.com"));

            Assert.NotNull(usuario);
        }

        [Fact]
        public async Task NaoIncluirDoisUsuariosComMesmoEmail()
        {
            //Arranje
            var usuarioService = GetUsuarioService();

            var usuario = new Usuario("Camila Silva", "camila.silva@gmail.com");
            var mesmoUsuario1 = await usuarioService.InsertAsync(usuario);

            //Act - método sob teste.
            var mesmoUsuario2 = await usuarioService.InsertAsync(usuario);

            //Assert
            Assert.Equal(mesmoUsuario1.Id, mesmoUsuario2.Id);
        }


        UsuarioRepository _usuarioRepository = null;
        private UsuarioRepository GetUsuarioRepository()
        {
            if(_usuarioRepository != null)
                return _usuarioRepository;

            return new UsuarioRepository(DatabaseTest.Start());
        }

        UsuarioService _usuarioService = null;
        private UsuarioService GetUsuarioService()
        {
            if(_usuarioService != null)
                return _usuarioService;

            return new UsuarioService(this.GetUsuarioRepository());
        }
    }
}