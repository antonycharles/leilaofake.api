using LeilaoFake.Me.Core.Exceptions;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class LanceConstrutorTest
    {
        [Fact]
        public void LancaValorLanceNegativoException()
        {
            //Arranje
            var interessado = new Usuario("789456", "Camila Silva");
            var valorNegativo = -100;

            //Assert
            Assert.Throws<ValorNegativoException>(
                //Act
                () => new Lance(null, interessado, valorNegativo, null)
            );
        }

        [Fact]
        public void LancaArgumentNullExceptionInterassadoNaoInformado()
        {
            //Arranje
            var valor = 100;

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Lance(null, null, valor, null)
            );
        }
    }
}
