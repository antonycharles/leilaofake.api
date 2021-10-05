using System;
using System.Data;
using System.Threading.Tasks;
using Bogus;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Datas.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories
{
    public class LeilaoImagemRepositoryTest
    {
        private readonly IDbConnection _dbConnection;

        public LeilaoImagemRepositoryTest()
        {
            var dataBaseTest = new DataBaseTest("db_leilaofake_test_leilao_imagem_repository");
            //dataBaseTest.CreateDataBaseTest();

            var serviceProvider = dataBaseTest.CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                dataBaseTest.UpdateDatabase(scope.ServiceProvider);
            }

            _dbConnection = dataBaseTest.GetConnection();
        }

        [Fact]
        public async Task IncluirLeilaoImagemComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoImagemRepository = new LeilaoImagemRepository(_dbConnection);
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));
            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 250.50)
            );

            //Act
            var leilaoImagemId = await leilaoImagemRepository.InsertAsync(
                new LeilaoImagem(usuarioId, leilaoId,"url-da-imagem.png")
            );

            //Assert
            Assert.NotNull(leilaoImagemId);
        }

        [Fact]
        public async Task DeleteLeilaoImagemComSucesso()
        {
             //Arranje
            var faker = new Faker("pt_BR");
            var leilaoImagemRepository = new LeilaoImagemRepository(_dbConnection);
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));
            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 250.50)
            );


            var leilaoImagemId = await leilaoImagemRepository.InsertAsync(
                new LeilaoImagem(usuarioId, leilaoId,"url-da-imagem.png")
            );

            //Act
            await leilaoImagemRepository.DeleteAsync(leilaoImagemId);
            
        }
    }
}