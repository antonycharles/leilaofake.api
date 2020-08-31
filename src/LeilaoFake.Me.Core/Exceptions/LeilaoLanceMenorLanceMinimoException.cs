using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoLanceMenorLanceMinimoException : Exception
    {
        public LeilaoLanceMenorLanceMinimoException()
        {
        }

        public LeilaoLanceMenorLanceMinimoException(string message)
            : base(message)
        {
        }

        public LeilaoLanceMenorLanceMinimoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
