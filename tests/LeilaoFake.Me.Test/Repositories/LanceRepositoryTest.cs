using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories
{
    public class LanceRepositoryTest : IClassFixture<CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup> _factory;
        private readonly IDbConnection _dbConnection;

        public LanceRepositoryTest(CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _dbConnection = factory.GetConnection();
        }

        [Fact]
        public async Task IncluirLanceComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var LanceRepository = new LanceRepository(_dbConnection);
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
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