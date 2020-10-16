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
                    leilao.RecebeLance(new Lance(null, bruno, lanceValores[i],leilao.Id));
                }
                else
                {
                    leilao.RecebeLance(new Lance(null, andre, lanceValores[i], leilao.Id));
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

            //Assert
            Assert.Throws<LeilaoUltimoLanceMesmoClienteException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, bruno, 500, leilao.Id))
            );
        }

        [Fact]
        public void NaoPermiteLancesCasoLeilaoNaoEstejaEmAndamento()
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            //Assert
            Assert.Throws<LeilaoNaoEstaEmAndamentoException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, andre, 300, leilao.Id))
            );
        }

        [Fact]
        public void NaoPermitirLancesAntesDoInicioDoLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceForaDoPrazoException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, andre, 300, leilao.Id))
            );
        }

        [Fact]
        public void NaoPermitirLancesAposDataFimDoLeilao()
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-4);
            var fim = DateTime.Now.AddDays(-2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceForaDoPrazoException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, andre, 300, leilao.Id))
            );
        }

        [Fact]
        public void NaoPermitirLancesDoLeiloador()
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoNaoPermiteLanceDoLeiloadorException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, leiloadoPor, 300, leilao.Id))
            );
        }

        [Fact]
        public void NaoPermitirLancesMenoresQueValorDoLanceMinimo()
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

            //Assert
            Assert.Throws<LeilaoLanceMenorLanceMinimoException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, andre, 100, leilao.Id))
            );
        }

        [Theory]
        [InlineData(new double[] { 800, 900, 1000, 1200, 1400}, 1350)]
        [InlineData(new double[] { 800, 900, 1000, 1100 }, 1100)]
        [InlineData(new double[] { 800 }, 799)]
        public void NaoPermiteNovoLancesMenoresQueUltimoLance(double[] lanceValores, double ultimoLanceValor)
        {
            //Arranje - cenário
            var leiloadoPor = new Usuario("789456", "Camila Silva");
            var andre = new Usuario("1234456789", "Andre Mattos");
            var bruno = new Usuario("789456123", "Bruno Gomes");
            var evandro = new Usuario("789-789-789", "Evandro Silva");

            var titulo = "Novo Leilão";
            var inicio = DateTime.Now.AddDays(-1);
            var fim = DateTime.Now.AddDays(2);
            var lanceMinimo = 200;
            var leilao = new Leilao(null, leiloadoPor, titulo, inicio, fim, lanceMinimo);

            leilao.IniciaPregao();

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

            //Assert
            Assert.Throws<LeilaoLanceMenorLanceMinimoException>(
                //Act
                () => leilao.RecebeLance(new Lance(null, evandro, ultimoLanceValor, leilao.Id))
            );
        }
    }
}
