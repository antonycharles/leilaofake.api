using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoResponse
    {
        private readonly IUrlHelper _urlHelper;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly Leilao _leilao;

        public string Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public UsuarioResponse LeiloadoPor { get; private set; }
        public bool IsPublico { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AlteradoEm { get; private set; }
        public int TotalLances { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public double LanceMinimo { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }
        public IList<LanceResponse> Lances { get; private set; }
        public IList<LeilaoImagemResponse> LeilaoImagens { get; private set; }
        public string LanceGanhadorId { get; private set; }
        public LanceResponse LanceGanhador { get; private set; }
        public string Status { get; private set; }
        public IList<LinkResponse> Links { get; private set; } = new List<LinkResponse>();
        public string LinkCaminhoImagem { get; private set; }

        public LeilaoResponse(Leilao leilao, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            _urlHelper = urlHelper;
            _usuarioAutenticado = usuarioAutenticado;
            _leilao = leilao;

            Id = leilao.Id;
            LeiloadoPorId = leilao.LeiloadoPorId;
            IsPublico = leilao.IsPublico;
            CriadoEm = leilao.CriadoEm;
            AlteradoEm = leilao.AlteradoEm;
            TotalLances = leilao.TotalLances;
            Titulo = leilao.Titulo;
            Descricao = leilao.Descricao;
            LanceMinimo = leilao.LanceMinimo;
            DataInicio = leilao.DataInicio;
            DataFim = leilao.DataFim;
            LanceGanhadorId = leilao.LanceGanhadorId;
            Status = leilao.StatusString;
            LinkCaminhoImagem = this.CriarLinkImagem();

            if (leilao.LanceGanhador != null)
                LanceGanhador = new LanceResponse(leilao.LanceGanhador, _urlHelper, usuarioAutenticado);

            if (leilao.LeiloadoPor != null)
                LeiloadoPor = new UsuarioResponse(leilao.LeiloadoPor, _urlHelper, usuarioAutenticado);

            Lances = leilao.Lances.Select(s => new LanceResponse(s, _urlHelper, usuarioAutenticado)).ToList();
            LeilaoImagens = leilao.LeilaoImagems.Select(s => new LeilaoImagemResponse(s, _urlHelper, usuarioAutenticado)).ToList();
        }

        public void AddAllLinks()
        {
            AddLinkLeilaoId();
            AddLinkUpdate();
            AddLinkDelete();
            AddLinkIniciarPregao();
            AddLinkCancelar();
            AddLinkFinalizar();
            AddLinkTornarPublico();
            AddLinkTornarPrivado();
            AddLinkIncluirLance();
            AddLinkIncluirImagem();
        }

        public void AddLinkLeilaoId()
        {
            Links.Add(new LinkResponse(
                href: _urlHelper.ActionLink("GetId", "Leilao", new { leilaoId = _leilao.Id }),
                rel: "self",
                metodo: "GET"));
        }

        public void AddLinkUpdate()
        {
            if (_leilao.IsUpdate && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Update", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "update",
                    metodo: "PUT"));
            }
        }

        public void AddLinkDelete()
        {
            if (_leilao.IsDelete && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Delete", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "delete",
                    metodo: "DELETE"));
            }
        }

        public void AddLinkIniciarPregao()
        {
            if (_leilao.IsIniciaPregao && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("IniciarPregao", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "iniciar_pregao",
                    metodo: "PATCH"));
            }
        }

        public void AddLinkCancelar()
        {
            if (_leilao.IsCancelarLeilao && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Cancelar", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "cancelar",
                    metodo: "PATCH"));
            }
        }

        public void AddLinkFinalizar()
        {
            if (_leilao.IsFinalizarLeilao && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Finalizar", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "finalizar",
                    metodo: "PATCH"));
            }
        }

        public void AddLinkTornarPublico()
        {
            if (!_leilao.IsPublico && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("TornarPublico", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "tornar_publico",
                    metodo: "PATCH"));
            }
        }

        public void AddLinkTornarPrivado()
        {
            if (_leilao.IsPublico && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("TornarPrivado", "Leilao", new { leilaoId = _leilao.Id }),
                    rel: "tornar_privado",
                    metodo: "PATCH"));
            }
        }

        public void AddLinkIncluirLance()
        {
            if (_leilao.IsLanceDeveSerAceito && _usuarioAutenticado.IsAuthenticated)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Incluir", "Lance", new { }),
                    rel: "add_lance",
                    metodo: "POST"));
            }
        }

        public void AddLinkIncluirImagem()
        {
            if (_leilao.IsUpdate && _usuarioAutenticado.Id == _leilao.LeiloadoPorId)
            {
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Incluir", "LeilaoImagem"),
                    rel: "add_imagens",
                    metodo: "POST"));
            }
        }

        private string CriarLinkImagem()
        {
            if (_leilao.CaminhoImagem == null && _leilao.LeilaoImagems.Count() == 0)
                return "";

            var uriBuilder = new UriBuilder
            {
                Scheme = _urlHelper.ActionContext.HttpContext.Request.Scheme,
                Host = _urlHelper.ActionContext.HttpContext.Request.Host.Host
            };

            if(_leilao.CaminhoImagem != null)
                return uriBuilder.Uri + _leilao.CaminhoImagem;

            return _leilao.LeilaoImagems.Select(s => uriBuilder.Uri + s.Url).FirstOrDefault();
        }
    }
}