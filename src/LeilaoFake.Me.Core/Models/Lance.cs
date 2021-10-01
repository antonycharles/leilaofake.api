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
        public DateTime CriadoEm { get; private set; }

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

        private string _interessadoId;

        public string InteressadoId{ 
            get
            {
                return _interessadoId;
            } 
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("Interessado deve ser informado!");

                _interessadoId = value;
            }
        }

        private Usuario _interessado;
        public Usuario Interessado {
            get
            {
                return _interessado;
            }
            private set
            {
                

                _interessado = value;
                InteressadoId = value.Id;
            }
        }

        private string _leilaoId;
        public string LeilaoId { 
            get
            {
                return _leilaoId;
            } 
            private set
            {
                if(value == null)
                    throw new ArgumentNullException("O Leilão deve ser informado!");

                _leilaoId = value;
            } 
        }
        
        public Lance() { }
        public Lance(string interessadoId, double valor, string leilaoId)
        {
            InteressadoId = interessadoId;
            Valor = valor;
            CriadoEm = DateTime.UtcNow;
            LeilaoId = leilaoId;
        }
    }
}
