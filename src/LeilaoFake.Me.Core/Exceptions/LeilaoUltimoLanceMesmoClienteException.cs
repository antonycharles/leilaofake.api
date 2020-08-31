using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoUltimoLanceMesmoClienteException : Exception
    {
        public LeilaoUltimoLanceMesmoClienteException()
        {
        }

        public LeilaoUltimoLanceMesmoClienteException(string message)
            : base(message)
        {
        }

        public LeilaoUltimoLanceMesmoClienteException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
