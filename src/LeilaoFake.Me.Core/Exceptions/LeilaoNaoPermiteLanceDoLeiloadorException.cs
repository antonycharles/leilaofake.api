using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoNaoPermiteLanceDoLeiloadorException : Exception
    {
        public LeilaoNaoPermiteLanceDoLeiloadorException()
        {
        }

        public LeilaoNaoPermiteLanceDoLeiloadorException(string message)
            : base(message)
        {
        }

        public LeilaoNaoPermiteLanceDoLeiloadorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
