using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Exceptions;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class LeilaoConstrutorTest
    {
        [Fact]
        public void LeilaoArgumentNullExceptionTituloNull()
        {
            //Arranje
            var leiloadoPor = "789456";
            string titulo = null;
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo)
            );
        }

        [Fact]
        public void LeilaoConstrutorStatusEspera()
        {
            //Arranje
            var leiloadoPor = "789456";
            var titulo = "Novo Leilão";
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;

            //Act - método sob teste.
            var leilao = new Leilao(leiloadoPor,titulo,inicio,fim,lanceMinimo);

            //Assert
            var valorEsperado = StatusLeilaoEnum.Espera;
            var valorObtido = leilao.Status;
            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void LeilaoDataFimMenorQueDataInicioException()
        {
            //Arranje
            var leiloadoPor = "789456";
            string titulo = "Novo Leilão";
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(-1);
            var lanceMinimo = 200;

            //Assert
            Assert.Throws<LeilaoDataFimMenorQueDataInicioException>(
                //Act
                () => new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo)
            );
        }

        [Fact]
        public void LeilaoLanceMinimoValorNegativoException()
        {
            //Arranje
            var leiloadoPor = "789456";
            var titulo = "Novo Leilão";
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = -20;

            //Assert
            Assert.Throws<ValorNegativoException>(
                //Act
                () => new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo)
            );
        }
    }
}
