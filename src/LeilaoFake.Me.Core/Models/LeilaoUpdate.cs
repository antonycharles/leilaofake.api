using System;

namespace LeilaoFake.Me.Core.Models
{
    public class LeilaoUpdate
    {
        public LeilaoUpdate(string leilaoId, string leiloadoPorId, string titulo, string descricao, double? lanceMinimo, DateTime? dataInicio, DateTime? dataFim)
        {
            LeilaoId = leilaoId;
            LeiloadoPorId = leiloadoPorId;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.LanceMinimo = lanceMinimo;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
        }

        public string LeilaoId { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public double? LanceMinimo { get; private set; }
        public DateTime? DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }
    }
}