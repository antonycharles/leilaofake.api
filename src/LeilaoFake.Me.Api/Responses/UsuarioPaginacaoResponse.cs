using System.Collections.Generic;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Api.Requests
{
    public class UsuarioPaginacaoResponse
    {
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IList<Usuario> Resultado { get; set; }
    }
}