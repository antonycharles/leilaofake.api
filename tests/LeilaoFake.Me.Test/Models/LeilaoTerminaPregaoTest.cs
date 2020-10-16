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
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");
            var bruno = new Usuario("789456123", "Bruno Gomes");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            leilao.RecebeLance(new Lance(null, andre, 300, leilao.Id));
            leilao.RecebeLance(new Lance(null, bruno, 400, leilao.Id));

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
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");
            var bruno = new Usuario("789456123", "Bruno Gomes");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

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
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");
            var bruno = new Usuario("789456123", "Bruno Gomes");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            var lance1 = new Lance("1111", andre, 300, leilao.Id);
            leilao.RecebeLance(lance1);

            var lance2 = new Lance("4444", bruno, 400, leilao.Id);
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
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");
            var bruno = new Usuario("789456123", "Bruno Gomes");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Act - método sob teste.
            for (int i = 0; i < lanceValores.Length; i++)
            {
                if (i % 2 == 0)
                {
                    leilao.RecebeLance(new Lance(null, bruno, lanceValores[i], leilao.Id));
                }
                else
                {
                    leilao.RecebeLance(new Lance(null, andre, lanceValores[i], leilao.Id));
                }
            }

            leilao.FinalizarLeilao();

            //Assert
            var valorObtido = leilao.LanceGanhador.Valor;
            Assert.Equal(maiorLance, valorObtido);
        }

    }
}