﻿using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class LeilaoIniciaPregaoTest
    {
        [Fact]
        public void AlteraStatusLeilaoParaEmAndamento()
        {
            //Arranje
            var leiloadoPor = "789456";
            var titulo = "Novo Leilão";
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            //Act - método sob teste.
            leilao.IniciaPregao();

            //Assert
            var valorEsperado = StatusLeilaoEnum.EmAndamento;
            var valorObtido = leilao.Status;
            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void AlteraStatusLeilaoCanceladoParaEmAndamentoArgumentException()
        {
            //Arranje
            var leiloadoPor = "789456";
            var titulo = "Novo Leilão";
            var inicio = DateTime.Now;
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);
            leilao.CancelarLeilao();

            //Assert
            Assert.Throws<ArgumentException>(
                //Act
                () => leilao.IniciaPregao()
            );
        }
    }
}
