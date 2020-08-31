using System;
using System.Collections.Generic;
using System.Text;

namespace LeilaoFake.Me.Core.Exceptions
{
    public class LeilaoDataFimMenorQueDataInicioException : Exception
    {
        public LeilaoDataFimMenorQueDataInicioException()
        {
        }

        public LeilaoDataFimMenorQueDataInicioException(string message)
            : base(message)
        {
        }

        public LeilaoDataFimMenorQueDataInicioException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
