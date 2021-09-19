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
            var nome = "789456";

            //Assert
            Usuario usuario = new Usuario(nome,"camila.silva@gmail.com");

            //Act
            Assert.Equal(usuario.Nome, nome);
        }
    }
}