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
    public class LeilaoImagemRepositoryTest : RepositoryTests
    {

        public LeilaoImagemRepositoryTest() : base("db_lf_test_leilao_imagem_re", true)
        {
        }

        [Fact]
        public async Task IncluirLeilaoImagemComSucessoAsync()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            var leilaoImagemRepository = new LeilaoImagemRepository(this.Context);
            var leilaoRepository = new LeilaoRepository(this.Context);
            var usuarioRepository = new UsuarioRepository(this.Context);
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
            var leilaoImagemRepository = new LeilaoImagemRepository(this.Context);
            var leilaoRepository = new LeilaoRepository(this.Context);
            var usuarioRepository = new UsuarioRepository(this.Context);
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