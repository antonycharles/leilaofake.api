using Bogus;
using LeilaoFake.Me.Core.Exceptions;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories
{
    public class UsuarioRepositoryTest
    {
        [Fact]
        public async Task IncluirNovoUsuarioComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();

            //Act
            var usuario = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Assert
            Assert.NotNull(usuario);
        }

        [Fact]
        public async Task AlterarUsuarioComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();
            var usuario = new Usuario(faker.Name.FullName(), faker.Internet.Email());
            usuario = await usuarioRepository.InsertAsync(usuario);
            var usuarioUpdata = new Usuario(faker.Name.FullName(), faker.Internet.Email());

            usuario.Update(usuarioUpdata);

            //Act 
            await usuarioRepository.UpdateAsync(usuario);

            var usuarioAlterado = await usuarioRepository.GetByIdAsync(usuario.Id);

            Assert.Equal(usuarioUpdata.Nome, usuarioAlterado.Nome);
        }

        [Fact]
        public async Task GetUsuarioEmailComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();
            var usuario = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            var usuarioDb = await usuarioRepository.GetByEmailAsync(usuario.Email);

            //Assert
            Assert.Equal(usuario.Email, usuarioDb.Email);
        }

        [Fact]
        public async Task GetUsuarioByIdComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();
            var usuario = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            var usuarioDb = await usuarioRepository.GetByIdAsync(usuario.Id);

            //Assert
            Assert.Equal(usuario.Id, usuarioDb.Id);
        }

        [Fact]
        public async Task GetUsuariosAllComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();
            var totalUsuarios = 20;

            for(int i = 0; i < totalUsuarios; i++){
                var usuario = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));
            }

            //Act
            var usuarios = await usuarioRepository.GetAllAsync(new UsuarioPaginacao(20,1));

            //Assert
            Assert.Equal(usuarios.Resultados.Count, totalUsuarios);
        }

        [Fact]
        public async Task DeleteUsuarioComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = GetUsuarioRepository();
            var usuario = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            await usuarioRepository.DeleteAsync(usuario.Id);

            //Assert
            Assert.Null(await usuarioRepository.GetByIdAsync(usuario.Id));
        }

        UsuarioRepository _usuarioRepository = null;
        private UsuarioRepository GetUsuarioRepository()
        {
            if(_usuarioRepository != null)
                return _usuarioRepository;

            return new UsuarioRepository(DatabaseTest.Start());
        }

    }
}