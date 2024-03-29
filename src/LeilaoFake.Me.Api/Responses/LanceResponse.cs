using System;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class LanceResponse
    {
        public string Id { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public double Valor  { get; private set; }
        public string LeilaoId { get; private set; }
        public UsuarioResponse Interessado { get; private set; }

        public LanceResponse(Lance lance, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            Id = lance.Id;
            CriadoEm = lance.CriadoEm;
            Valor = lance.Valor;
            LeilaoId = lance.LeilaoId;

            if(lance.Interessado != null)
                Interessado = new UsuarioResponse(lance.Interessado.Id,lance.Interessado.Nome);
        }

    }
}