using System;
using System.Collections.Generic;
using System.Security.Policy;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoResponse
    {
        private readonly IUrlHelper _urlHelper;

        public string Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public Usuario LeiloadoPor { get; private set; }
        public bool IsPublico {get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AlteradoEm { get; private set; }
        public int TotalLances { get; private set; }
        public string Titulo { get; private set;}
        public string Descricao { get; private set; }
        public double LanceMinimo { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }
        public IList<Lance> Lances { get; private set; }
        public string LanceGanhadorId { get; private set; }
        public Lance LanceGanhador { get; private set; }
        public string Status { get; private set; }
        public IList<LinkResponse> Links { get; private set; } = new List<LinkResponse>();

        public LeilaoResponse(Leilao leilao, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;

            Id = leilao.Id;
            LeiloadoPorId = leilao.LeiloadoPorId;
            LeiloadoPor = leilao.LeiloadoPor;
            IsPublico = leilao.IsPublico;
            CriadoEm = leilao.CriadoEm;
            AlteradoEm = leilao.AlteradoEm;
            TotalLances = leilao.TotalLances;
            Titulo = leilao.Titulo;
            Descricao = leilao.Descricao;
            LanceMinimo = leilao.LanceMinimo;
            DataInicio = leilao.DataInicio;
            DataFim = leilao.DataFim;
            Lances = leilao.Lances;
            LanceGanhadorId = leilao.LanceGanhadorId;
            LanceGanhador = leilao.LanceGanhador;
            Status = leilao.StatusString;

            this.CreateLinks(leilao);
        }

        public void CreateLinks(Leilao leilao)
        {
            Links.Add(new LinkResponse(
                href: _urlHelper.ActionLink("GetId","Leilao", new {leilaoId = leilao.Id}),
                rel: "self", 
                metodo: "GET"));

            if(leilao.IsUpdate){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Update","Leilao", new {leilaoId = leilao.Id}),
                    rel: "update", 
                    metodo: "PUT"));
            }

            if(leilao.IsDelete){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Delete","Leilao", new {leilaoId = leilao.Id}),
                    rel: "delete", 
                    metodo: "DELETE"));
            }

            if(leilao.IsIniciaPregao){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("IniciarPregao","Leilao", new {leilaoId = leilao.Id}),
                    rel: "iniciar_pregao", 
                    metodo: "PATCH"));
            }

            if(leilao.IsCancelarLeilao){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Cancelar","Leilao", new {leilaoId = leilao.Id}),
                    rel: "cancelar", 
                    metodo: "PATCH"));
            }

            if(leilao.IsFinalizarLeilao){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Finalizar","Leilao", new {leilaoId = leilao.Id}),
                    rel: "finalizar", 
                    metodo: "PATCH"));
            }

            if(!leilao.IsPublico){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("TornarPublico","Leilao", new {leilaoId = leilao.Id}),
                    rel: "tornar_publico", 
                    metodo: "PATCH"));
            }

            if(leilao.IsPublico){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("TornarPrivado","Leilao", new {leilaoId = leilao.Id}),
                    rel: "tornar_privado", 
                    metodo: "PATCH"));
            }

            if(leilao.IsLanceDeveSerAceito){
                Links.Add(new LinkResponse(
                    href: _urlHelper.ActionLink("Incluir","Lance", new {}),
                    rel: "add_lance", 
                    metodo: "POST"));
            }
        }
    }
}