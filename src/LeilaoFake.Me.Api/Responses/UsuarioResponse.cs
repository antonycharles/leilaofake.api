using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Responses
{
    public class UsuarioResponse
    {
        public UsuarioResponse(Usuario usuario, IUrlHelper urlHelper, UsuarioAutenticado usuarioAutenticado)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
        }

        public UsuarioResponse(string id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public string Id { get; private set;}
        public string Nome { get; private set; }
        public string Email { get; private set; }
    }
}