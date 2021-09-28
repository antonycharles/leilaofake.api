using System.Linq;
using System.Security.Claims;

namespace LeilaoFake.Me.Core.Models
{
    public class UsuarioAutenticado
    {
        public UsuarioAutenticado(ClaimsPrincipal usuario)
        {
            this.Id = usuario.Claims.Where(w => w.Type == ClaimTypes.Sid).Select(s => s.Value).FirstOrDefault();
            this.Nome = usuario.Claims.Where(w => w.Type == ClaimTypes.Name).Select(s => s.Value).FirstOrDefault();
            this.Email = usuario.Claims.Where(w => w.Type == ClaimTypes.Email).Select(s => s.Value).FirstOrDefault();
            this.Role = usuario.Claims.Where(w => w.Type == ClaimTypes.Role).Select(s => s.Value).FirstOrDefault();
            this.IsAuthenticated = usuario.Identity.IsAuthenticated;
        }

        public string Id { get; private set;}
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
}