using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class LeilaoTerminaPregaoTest
    {
        [Fact]
        public void DefineStatusLeilaoParaFinalizadoAoFinalizarLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            leilao.RecebeLance(new Lance(andre, 300, "456456"));
            leilao.RecebeLance(new Lance(bruno, 400, "456456"));

            //Act - método sob teste.
            leilao.FinalizarLeilao();


            //Assert
            var valorObtido = leilao.Status;
            Assert.Equal(StatusLeilaoEnum.Finalizado, valorObtido);
        }

        [Fact]
        public void LeilaoArgumentExceptionAoTentarFinalizarLeilaoSemSerIniciado()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo);

            // Assert
            Assert.Throws<ArgumentException>(
                //Act
                () => leilao.FinalizarLeilao()
            );
        }

        [Fact]
        public void DefineGanhadorIdAoFinalizarLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            var lance1 = new Lance(andre, 300, "456456");
            leilao.RecebeLance(lance1);

            var lance2 = new Lance(bruno, 400, "456456");
            leilao.RecebeLance(lance2);

            //Act - método sob teste.
            leilao.FinalizarLeilao();


            //Assert
            var valorObtido = leilao.LanceGanhadorId;
            Assert.Equal(lance2.Id,valorObtido);
        }


        [Theory]
        [InlineData(1500, new double[] { 800, 900, 1000, 1200, 1400, 1500 })]
        [InlineData(1100, new double[] { 800, 900, 1000, 1100 })]
        [InlineData(800, new double[] { 800 })]
        public void DefineMaiorLanceComoGanhadorAoFinalizarLeilao(double maiorLance, double[] lanceValores)
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Act - método sob teste.
            for (int i = 0; i < lanceValores.Length; i++)
            {
                if (i % 2 == 0)
                {
                    leilao.RecebeLance(new Lance(bruno, lanceValores[i], "456456"));
                }
                else
                {
                    leilao.RecebeLance(new Lance(andre, lanceValores[i], "456456"));
                }
            }

            leilao.FinalizarLeilao();

            //Assert
            var valorObtido = leilao.LanceGanhador.Valor;
            Assert.Equal(maiorLance, valorObtido);
        }

    }
}