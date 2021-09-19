using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LeilaoFake.Me.Core.Models
{
    [Table("leiloes")]
    public class Leilao
    {
        public string Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public Usuario LeiloadoPor { get; private set; }

        private string _titulo;
        public string Titulo {
            get
            {
                return _titulo;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("Título do leilão deve ser preenchido!");

                _titulo = value;
            }
        }

        public string Descricao { get; private set; }

        private double _lanceMinimo;
        public double LanceMinimo {
            get
            {
                return _lanceMinimo;
            }
            private set
            {
                if (value < 0)
                    throw new ValorNegativoException("Valor do lance mínimo não pode ser negativo!");

                _lanceMinimo = value;
            }
        }
        public DateTime DataInicio { get; private set; }

        private DateTime? _dataFim;
        public DateTime? DataFim 
        {
            get
            {
                return _dataFim;
            }

            private set
            {
                if (value < this.DataInicio)
                    throw new LeilaoDataFimMenorQueDataInicioException("Data fim informada menor que Data início!");

                 _dataFim = value;
            }
        }

        public IList<Lance> Lances { get; private set; } = new List<Lance>();
        public StatusLeilaoEnum Status { get; private set; }
        public string StatusString
        {
            get
            {
                return this.Status.GetDisplayName();
            }
        }

        public string LanceGanhadorId { get; private set; }
        public Lance LanceGanhador { get; private set; }

        public Leilao() { }

        public Leilao(string leiloadoPorId, string titulo,  DateTime inicio, DateTime fim, double lanceMinimo)
        {
            LeiloadoPorId = leiloadoPorId;
            Titulo = titulo;
            DataInicio = inicio;
            DataFim = fim;
            LanceMinimo = lanceMinimo;
            Status = StatusLeilaoEnum.Espera;
        }

        public void IniciaPregao()
        {
            if (StatusLeilaoEnum.Espera != this.Status)
                throw new ArgumentException("Não foi possível iníciar o leilão");

            this.Status = StatusLeilaoEnum.EmAndamento;
        }

        public void RecebeLance(Lance lance)
        {
            if (this.LanceDeveSerAceito(lance))
            {
                this.Lances.Add(lance);
            }
        }

        public void FinalizarLeilao()
        {
            if (this.Status != StatusLeilaoEnum.EmAndamento)
                throw new ArgumentException("Não é possível finalizar leilão que não está em andamento");

            this.Status = StatusLeilaoEnum.Finalizado;

            var maiorLance = this.MaiorLance();

            if (maiorLance != null)
            {
                this.LanceGanhadorId = maiorLance.Id;
                this.LanceGanhador = maiorLance;
            }
        }

        public void CancelarLeilao()
        {
            if (this.Status == StatusLeilaoEnum.Finalizado)
                throw new ArgumentException("Não é possível cancelar um leilão já finalizado");

            this.Status = StatusLeilaoEnum.Cancelado;
        }

        private bool LanceDeveSerAceito(Lance lance)
        {
            if (lance.InteressadoId == this.LeiloadoPorId)
                throw new LeilaoNaoPermiteLanceDoLeiloadorException("Lance inválido, interessado não pode dra lance neste item!");

            if (this.DataInicio > lance.Data || this.DataFim < lance.Data)
                throw new LeilaoLanceForaDoPrazoException("Lance fora do prazo de início ou fim!");

            if (this.Status != StatusLeilaoEnum.EmAndamento)
                throw new LeilaoNaoEstaEmAndamentoException($"Lance inválido, status do leilão não permite lances");

            if (this.LanceMinimo >= lance.Valor)
                throw new LeilaoLanceMenorLanceMinimoException("Lance inválido, valor menor que o mínimo");

            var maiorLance = this.MaiorLance();

            if (maiorLance != null && maiorLance.InteressadoId == lance.InteressadoId)
                throw new LeilaoUltimoLanceMesmoClienteException("Lance inválido, interessado já deu o último lance registrado");

            if(maiorLance != null && maiorLance.Valor >= lance.Valor)
                throw new LeilaoLanceMenorLanceMinimoException("Lance inválido, valor menor que último lance");

            return true;
        }

        private Lance MaiorLance()
        {
            return this.Lances
                        .OrderByDescending(x => x.Valor)
                        .OrderByDescending(x => x.Data)
                        .FirstOrDefault();
        }

    }
}