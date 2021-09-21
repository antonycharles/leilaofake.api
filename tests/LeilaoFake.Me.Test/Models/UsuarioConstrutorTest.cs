using Bogus;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class UsuarioConstrutorTest
    {
        [Fact]
        public void UsuarioConstrutorSucesso()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            string nome = faker.Name.FullName();
            string email = faker.Internet.Email();

            //Assert
            Usuario usuario = new Usuario(nome,email);

            //Act
            Assert.Equal(usuario.Nome, nome);
        }

        [Fact]
        public void UsuarioConstrutorEmailArgumentNullException()
        {
            //Arranje
            var faker = new Faker("pt_BR");
            string nome = faker.Name.FullName();
            string email = null;

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Usuario(nome,email)
            );
        }
    }
}