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
    public class LanceRepositoryTest : RepositoryTests
    {
        public LanceRepositoryTest() : base("db_lf_test_lance_re", true)
        {
        }

        [Fact]
        public async Task IncluirLanceComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var usuarioRepository = new UsuarioRepository(this.Context);
            var LanceRepository = new LanceRepository(this.Context,usuarioRepository);
            var leilaoRepository = new LeilaoRepository(this.Context);
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