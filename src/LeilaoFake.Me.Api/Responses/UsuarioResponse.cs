using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Api.Responses
{
    public class UsuarioResponse
    {
        public UsuarioResponse(Usuario usuario)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
        }

        public string Id { get; private set;}
        public string Nome { get; private set; }
        public string Email { get; private set; }
    }
}