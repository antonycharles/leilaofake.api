using System;
using System.Collections.Generic;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoImagemResponse
    {
        private readonly IUrlHelper _urlHelper;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public int Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public string LeilaoId { get; private set; }
        public string Url { get; private set; }

        public IList<LinkResponse> Links { get; private set; } = new List<LinkResponse>();

        public LeilaoImagemResponse(LeilaoImagem leilaoImagem, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            _urlHelper = urlHelper;
            _usuarioAutenticado = usuarioAutenticado;

            Id = leilaoImagem.Id;
            LeiloadoPorId = leilaoImagem.LeiloadoPorId;
            LeilaoId = leilaoImagem.LeilaoId;
            Url = leilaoImagem.Url;
            this.AddLinkLeilaoId();
            this.AddLinkDelete();
        }

        public void AddLinkLeilaoId()
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlHelper.ActionContext.HttpContext.Request.Scheme,
                Host = _urlHelper.ActionContext.HttpContext.Request.Host.Host,
                Port = _urlHelper.ActionContext.HttpContext.Request.Host.Port.GetValueOrDefault(80),
                Path = _urlHelper.ActionContext.HttpContext.Request.Path.ToString(),
                Query = _urlHelper.ActionContext.HttpContext.Request.QueryString.ToString()
            };

            Links.Add(new LinkResponse(
                href: uriBuilder.Uri + this.Url,
                rel: "self",
                metodo: "GET"));
        }

        public void AddLinkDelete()
        {
            Links.Add(new LinkResponse(
                href: _urlHelper.ActionLink("Delete", "LeilaoImagem", new { leilaoId = this.LeilaoId,leilaoImagemId = this.Id }),
                rel: "delete",
                metodo: "DELETE"));
        }
    }
}