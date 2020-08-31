using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoLanceForaDoPrazoException : Exception
    {
        public LeilaoLanceForaDoPrazoException()
        {
        }

        public LeilaoLanceForaDoPrazoException(string message)
            : base(message)
        {
        }

        public LeilaoLanceForaDoPrazoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
