using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoNaoEstaEmAndamentoException : Exception
    {
        public LeilaoNaoEstaEmAndamentoException()
        {
        }

        public LeilaoNaoEstaEmAndamentoException(string message)
            : base(message)
        {
        }

        public LeilaoNaoEstaEmAndamentoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
