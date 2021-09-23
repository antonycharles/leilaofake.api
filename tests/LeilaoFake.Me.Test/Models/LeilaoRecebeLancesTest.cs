using LeilaoFake.Me.Core.Exceptions;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LeilaoFake.Me.Test.Models
{
    public class LeilaoRecebeLancesTest
    {

        [Theory]
        [InlineData(new double[] { 800, 900, 1000, 1200, 1400, 1500 })]
        [InlineData(new double[] { 800, 900, 1000, 1100 })]
        [InlineData(new double[] { 800 })]
        public void AceitaTodosOsLances(double[] lanceValores)
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Act - método sob teste.
            for (int i = 0; i < lanceValores.Length; i++)
            {
                if (i % 2 == 0)
                {
                    leilao.RecebeLance(new Lance(bruno, lanceValores[i],"456456"));
                }
                else
                {
                    leilao.RecebeLance(new Lance(andre, lanceValores[i], "456456"));
                }
            }

            //Assert
            var valorObtido = leilao.Lances.Count();
            Assert.Equal(lanceValores.Length, valorObtido);
        }

        [Fact]
        public void NaoAceitaProximoLanceDadoMesmoClienteExecutouUltimoLanca()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            leilao.RecebeLance(new Lance(andre, 300, "456456"));
            leilao.RecebeLance(new Lance(bruno, 400, "456456"));

            //Assert
            Assert.Throws<LeilaoUltimoLanceMesmoClienteException>(
                //Act
                () => leilao.RecebeLance(new Lance(bruno, 500, "456456"))
            );
        }

        [Fact]
        public void NaoPermiteLancesCasoLeilaoNaoEstejaEmAndamento()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            //Assert
            Assert.Throws<LeilaoNaoEstaEmAndamentoException>(
                //Act
                () => leilao.RecebeLance(new Lance(andre, 300, "456456"))
            );
        }

        [Fact]
        public void NaoPermitirLancesAntesDoInicioDoLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceForaDoPrazoException>(
                //Act
                () => leilao.RecebeLance(new Lance(andre, 300, "456456"))
            );
        }

        [Fact]
        public void NaoPermitirLancesAposDataFimDoLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-4);
            var fim = DateTime.Now.AddDays(-2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceForaDoPrazoException>(
                //Act
                () => leilao.RecebeLance(new Lance(andre, 300, "456456"))
            );
        }

        [Fact]
        public void NaoPermitirLancesDoLeiloador()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoNaoPermiteLanceDoLeiloadorException>(
                //Act
                () => leilao.RecebeLance(new Lance(leiloadoPor, 300, "456456"))
            );
        }

        [Fact]
        public void NaoPermitirLancesMenoresQueValorDoLanceMinimo()
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceMenorLanceMinimoException>(
                //Act
                () => leilao.RecebeLance(new Lance(andre, 100, "456456"))
            );
        }

        [Theory]
        [InlineData(new double[] { 800, 900, 1000, 1200, 1400}, 1350)]
        [InlineData(new double[] { 800, 900, 1000, 1100 }, 1100)]
        [InlineData(new double[] { 800 }, 799)]
        public void NaoPermiteNovoLancesMenoresQueUltimoLance(double[] lanceValores, double ultimoLanceValor)
        {
            //Arranje - cenário
            var leiloadoPor = "789456";
            var andre = "1234456789";
            var bruno = "789456123";
            var evandro = "789-789-789";

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(leiloadoPor, titulo, null, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

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

            //Assert
            Assert.Throws<LeilaoLanceMenorLanceMinimoException>(
                //Act
                () => leilao.RecebeLance(new Lance(evandro, ultimoLanceValor, "456456"))
            );
        }
    }
}
