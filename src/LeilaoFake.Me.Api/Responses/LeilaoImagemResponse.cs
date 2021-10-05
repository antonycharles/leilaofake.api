using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class LeilaoImagemResponse
    {
        public int Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public string LeilaoId { get; private set; }
        public string Url { get; private set; }

        public LeilaoImagemResponse(LeilaoImagem leilaoImagem, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            Id = leilaoImagem.Id;
            LeiloadoPorId = leilaoImagem.LeiloadoPorId;
            LeilaoId = leilaoImagem.LeilaoId;
            Url = leilaoImagem.Url;
        }
    }
}