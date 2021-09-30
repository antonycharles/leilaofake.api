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
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LeilaoFake.Me.Test.Repositories 
{
    public class LeilaoRepositoryTest
    {
        private readonly IDbConnection _dbConnection;

        public LeilaoRepositoryTest()
        {
            var dataBaseTest = new DataBaseTest("db_leilaofake_test_leilao_repository");
            //dataBaseTest.CreateDataBaseTest();

            var serviceProvider = dataBaseTest.CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                dataBaseTest.UpdateDatabase(scope.ServiceProvider);
            }

            _dbConnection = dataBaseTest.GetConnection();
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
            LeilaoPaginacao leiloes = await leilaoRepository.GetAllAsync(new LeilaoPaginacao(porPagina:20, pagina:1, leiloadoPorId:usuarioId));

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
            leilao.Update(new LeilaoUpdate(leilaoId, usuarioId, leilaoTituloUpdate,null, null, null, null));
            //Act
            await leilaoRepository.UpdateAsync(leilao);

            //Assert
            Leilao leilaoUpdate = await leilaoRepository.GetByIdAsync(leilaoId);
            Assert.Equal(leilaoTituloUpdate, leilaoUpdate.Titulo);
        }
    }
}