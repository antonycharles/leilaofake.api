using System;

namespace LeilaoFake.Me.Core.Models
{
    public class Usuario
    {
        public string Id { get; private set;}
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public Usuario() { }
        public Usuario(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public void Update(Usuario usuario)
        {
            this.Nome = usuario.Nome;
        }
    }
}