using System.Collections.Generic;
using System.Linq;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoPaginacaoResponse
    {
        private readonly IUrlHelper _urlHelper;

        public string Search { get; private set; }
        public int Pagina { get; private set; }
        public int? Total { get; private set; }
        public int PorPagina { get; private set; }
        public string Order { get; private set; }
        public IList<LeilaoResponse> Resultados { get; private set; }

        public LeilaoPaginacaoResponse(LeilaoPaginacao leilaoPaginacao, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            
            Search = leilaoPaginacao.Search;
            Pagina = leilaoPaginacao.Pagina;
            Total = leilaoPaginacao.Total;
            PorPagina = leilaoPaginacao.PorPagina;
            Order = leilaoPaginacao.Order;
            Resultados = leilaoPaginacao.Resultados.Select(x => new LeilaoResponse(x,urlHelper)).ToList();
        }
    }
}