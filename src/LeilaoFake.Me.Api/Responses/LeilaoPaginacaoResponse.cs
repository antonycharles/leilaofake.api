using System.Collections.Generic;
using System.Linq;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoPaginacaoResponse
    {
        private readonly IUrlHelper _urlHelper;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public string Search { get; private set; }
        public int Pagina { get; private set; }
        public int? Total { get; private set; }
        public int PorPagina { get; private set; }
        public string Order { get; private set; }
        public IList<LeilaoResponse> Resultados { get; private set; }
        public IList<LinkResponse> Links { get; private set; } = new List<LinkResponse>();

        public LeilaoPaginacaoResponse(LeilaoPaginacao leilaoPaginacao, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            _urlHelper = urlHelper;
            _usuarioAutenticado = usuarioAutenticado;
            
            Search = leilaoPaginacao.Search;
            Pagina = leilaoPaginacao.Pagina;
            Total = leilaoPaginacao.Total;
            PorPagina = leilaoPaginacao.PorPagina;
            Order = leilaoPaginacao.Order;
            Resultados = leilaoPaginacao.Resultados.Select(x => {
                var leilao = new LeilaoResponse(x, urlHelper, usuarioAutenticado);
                leilao.AddAllLinks();
                return leilao;
            }).ToList();
        }

        public void AddLinkMeusLeiloes()
        {
            if(_usuarioAutenticado.IsAuthenticated){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetAllMeusLeiloes","Leilao"),
                    rel: "meus_leiloes", 
                    metodo: "GET"));
            }
        }

        public void AddLinkTodosLeiloes()
        {
            if(_usuarioAutenticado.IsAuthenticated){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetPaginacao","Leilao"),
                    rel: "meus_leiloes", 
                    metodo: "GET"));
            }
        }

        public void AddLinkProximaPagina()
        {
            var totalDePaginas = this.Total / this.PorPagina;

            if(totalDePaginas != 0 && totalDePaginas > this.Pagina){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetPaginacao","Leilao", new { Search, Pagina, PorPagina, Order}),
                    rel: "proxima_pagina", 
                    metodo: "GET"));
            }
        }

        public void AddLinkPaginaAnterior()
        {
            var totalDePaginas = this.Total / this.PorPagina;
            if(totalDePaginas != 0 && this.Pagina > 1){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetPaginacao","Leilao", new { Search, Pagina, PorPagina, Order}),
                    rel: "pagina_anterior", 
                    metodo: "GET"));
            }
        }

        public void AddLinkProximaPaginaUsuarioLogado()
        {
            var totalDePaginas = this.Total / this.PorPagina;

            if(totalDePaginas != 0 && totalDePaginas > this.Pagina){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetAllMeusLeiloes","Leilao", new { Search, Pagina, PorPagina, Order}),
                    rel: "proxima_pagina", 
                    metodo: "GET"));
            }
        }

        public void AddLinkPaginaAnteriorUsuarioLogado()
        {
            var totalDePaginas = this.Total / this.PorPagina;
            if(totalDePaginas != 0 && this.Pagina > 1){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("GetAllMeusLeiloes","Leilao", new { Search, Pagina, PorPagina, Order}),
                    rel: "pagina_anterior", 
                    metodo: "GET"));
            }
        }
    }
}