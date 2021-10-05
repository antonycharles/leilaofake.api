using System.Collections.Generic;
using System.Linq;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class UsuarioPaginacaoResponse
    {
        private readonly IUrlHelper _urlHelper;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public string Search { get; private set; }
        public int Pagina { get; private set; }
        public int? Total { get; private set; }
        public int PorPagina { get; private set; }
        public string Order { get; private set; }
        public IList<UsuarioResponse> Resultados { get; private set; }

        public UsuarioPaginacaoResponse(UsuarioPaginacao usuarioPaginacao, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            _urlHelper = urlHelper;
            _usuarioAutenticado = usuarioAutenticado;
            
            Pagina = usuarioPaginacao.Pagina;
            Total = usuarioPaginacao.Total;
            PorPagina = usuarioPaginacao.PorPagina;
            Order = usuarioPaginacao.Order;
            Resultados = usuarioPaginacao.Resultados.Select(x => {
                var usuario = new UsuarioResponse(x, urlHelper, usuarioAutenticado);
                return usuario;
            }).ToList();
        }
    }
}