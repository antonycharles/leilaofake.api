using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories
{
    public class UsuarioRepositoryTest
    {
        private readonly IDbConnection _dbConnection;

        public UsuarioRepositoryTest()
        {
            var dataBaseTest = new DataBaseTest("db_leilaofake_test_usuario_repository");
            //dataBaseTest.CreateDataBaseTest();


            var serviceProvider = dataBaseTest.CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                dataBaseTest.UpdateDatabase(scope.ServiceProvider);
            }

            _dbConnection = dataBaseTest.GetConnection();
        }

        [Fact]
        public async Task IncluirNovoUsuarioComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = new UsuarioRepository(_dbConnection);

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
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuario = new Usuario(faker.Name.FullName(), faker.Internet.Email());
            string usuarioId = await usuarioRepository.InsertAsync(usuario);
            usuario = await usuarioRepository.GetByIdAsync(usuarioId);
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
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            string nome = faker.Name.FullName();
            string email = faker.Internet.Email();
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(nome, email));

            //Act
            var usuarioDb = await usuarioRepository.GetByEmailAsync(email);

            //Assert
            Assert.Equal(email, usuarioDb.Email);
        }

        [Fact]
        public async Task GetUsuarioByIdComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            var usuarioDb = await usuarioRepository.GetByIdAsync(usuarioId);

            //Assert
            Assert.Equal(usuarioId, usuarioDb.Id);
        }

        [Fact]
        public async Task GetUsuariosAllComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = new UsuarioRepository(_dbConnection);
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
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            await usuarioRepository.DeleteAsync(usuarioId);

            //Assert
            Assert.Null(await usuarioRepository.GetByIdAsync(usuarioId));
        }
    }
}