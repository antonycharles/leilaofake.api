using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class ValorNegativoException : Exception
    {
        public ValorNegativoException()
        {
        }

        public ValorNegativoException(string message)
            : base(message)
        {
        }

        public ValorNegativoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
