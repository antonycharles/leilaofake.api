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
        public void LancaConstrutorSucesso()
        {
            //Arranje
            var interessado = "789456";
            var valor = 100;
            var leilao = "789456";

            //Assert
            Lance lance = new Lance(interessado, valor, leilao);

            //Act
            Assert.Equal(lance.InteressadoId, interessado);
            Assert.Equal(lance.Valor, valor);
            Assert.Equal(lance.LeilaoId, leilao);
        }

        [Fact]
        public void LancaValorLanceNegativoException()
        {
            //Arranje
            var interessado = "789456";
            var valorNegativo = -100;

            //Assert
            Assert.Throws<ValorNegativoException>(
                //Act
                () => new Lance(interessado, valorNegativo, null)
            );
        }

        [Fact]
        public void LancaArgumentNullExceptionInterassadoNaoInformado()
        {
            //Arranje
            var valor = 100;
            var leilao = "456789";

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Lance(null, valor, leilao)
            );
        }

        [Fact]
        public void LancaArgumentNullExceptionLeilaoNaoInformado()
        {
            //Arranje
            var valor = 100;
            var interessado = "789456";

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Lance(interessado, valor, null)
            );
        }
    }
}
