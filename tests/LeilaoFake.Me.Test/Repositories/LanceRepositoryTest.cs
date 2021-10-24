using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using LeilaoFake.Me.Api;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Datas.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories
{
    public class LanceRepositoryTest 
    {
        private readonly IDbConnection _dbConnection;

        public LanceRepositoryTest()
        {
            var dataBaseTest = new DataBaseTest("db_leilaofake_test_lance_repository");
            //dataBaseTest.CreateDataBaseTest();

            var serviceProvider = dataBaseTest.CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                dataBaseTest.UpdateDatabase(scope.ServiceProvider);
            }

            _dbConnection = dataBaseTest.GetConnection();
        }

        [Fact]
        public async Task IncluirLanceComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var LanceRepository = new LanceRepository(_dbConnection,usuarioRepository);
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));
            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 250.50)
            );

            //Act
            var lanceId = await LanceRepository.InsertAsync(
                new Lance(usuarioId,300,leilaoId)
            );

            //Assert
            Assert.NotNull(lanceId);
        }
        
    }
}