using LeilaoFake.Me.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeilaoFake.Me.Core.Models
{
    public class Lance
    {
        public string Id { get; private set; }
        public DateTime Data { get; private set; }

        private double _valor;
        public double Valor {
            get {
                return _valor;
            }
            private set {
                if (value < 0)
                    throw new ValorNegativoException("Valor do lance não pode ser negativo!");

                _valor = value;
            }
        }

        public string InteressadoId{ get; private set; }

        public Usuario _interessado;

        public Usuario Interessado {
            get
            {
                return _interessado;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("Interessado deve ser informado!");

                _interessado = value;
                InteressadoId = value.Id;
            }
        }

        public string LeilaoId { get; private set; }
        public Leilao Leilao { get; private set; }

        public Lance() { }
        public Lance(string id,Usuario interessado, double valor)
        {
            Id = (id == null ? Guid.NewGuid().ToString() : id);
            Interessado = interessado;
            Valor = valor;
            Data = DateTime.Now;
        }
    }
}
