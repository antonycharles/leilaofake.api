using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories 
{
    public class LeilaoRepositoryTest : IClassFixture<CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup> _factory;
        private readonly IDbConnection _dbConnection;

        public LeilaoRepositoryTest(CustomWebApplicationFactory<LeilaoFake.Me.Api.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _dbConnection = factory.GetConnection();
        }

        [Fact]
        public async Task IncluirLeilaoComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            //Act
            var leilao = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), 250.50)
            );

            //Assert
            Assert.NotNull(leilao);
        }
        
        [Fact]
        public async Task GetLeilaoIdComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão GetLeilaoIdComSucesso", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(50), 250.50)
            );

            //Act
            Leilao leilao = await leilaoRepository.GetByIdAsync(leilaoId);

            //Assert
            Assert.Equal(leilao.Id, leilaoId);
        }

        [Fact]
        public async Task GetLeiloadoPorIdComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão GetLeilaoIdComSucesso", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(50), 250.50)
            );

            //Act
            IList<Leilao> leiloes = await leilaoRepository.GetAllByLeiloadoPorIdAsync(usuarioId);

            //Assert
            Assert.NotNull(leiloes.Where(w => w.Id == leilaoId).FirstOrDefault());
        }

        [Fact]
        public async Task GetAllComSucesso()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            var totalLeiloes = 20;

            for(int i = 0; i < totalLeiloes; i++){
                var leilaoId = await leilaoRepository.InsertAsync(
                    new Leilao(usuarioId, faker.Internet.UserNameUnicode(), null, DateTime.UtcNow, DateTime.UtcNow.AddDays(50), 250.50)
                );
            }

            //Act
            LeilaoPaginacao leiloes = await leilaoRepository.GetAllAsync(new LeilaoPaginacao(20,1));

            //Assert
            Assert.Equal(leiloes.Resultados.Count, totalLeiloes);
        }

        [Fact]
        public async Task DeleteLeilaoComSucesso()
        {
            //Arranje
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão GetLeilaoIdComSucesso", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(50), 250.50)
            );

            //Act
            await leilaoRepository.DeleteAsync(leilaoId);
        }
        

        [Fact]
        public async Task UpdateLeilaoComSucesso()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoRepository = new LeilaoRepository(_dbConnection);
            var usuarioRepository = new UsuarioRepository(_dbConnection);
            var usuarioId = await usuarioRepository.InsertAsync(new Usuario(faker.Name.FullName(), faker.Internet.Email()));

            var leilaoId = await leilaoRepository.InsertAsync(
                new Leilao(usuarioId, "Teste leilão GetLeilaoIdComSucesso", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(50), 250.50)
            );


            Leilao leilao = await leilaoRepository.GetByIdAsync(leilaoId);

            string leilaoTituloUpdate = "Titulo leilão Update";
            leilao.Update(titulo:leilaoTituloUpdate);
            //Act
            await leilaoRepository.UpdateAsync(leilao);

            //Assert
            Leilao leilaoUpdate = await leilaoRepository.GetByIdAsync(leilaoId);
            Assert.Equal(leilaoTituloUpdate, leilaoUpdate.Titulo);
        }
    }
}