using System;

namespace LeilaoFake.Me.Core.Models
{
    public class Usuario
    {
        public string Id { get; private set;}
        public string Nome { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AlteradoEm { get; private set; } = DateTime.UtcNow;
        public string Role { get; private set; }

        private string _email;
        public string Email { 
            get
            {
                return _email;
            } 
            private set
            {
                if(value == null)
                    throw new ArgumentNullException("Valor do email não pode ser null");

                _email = value;
            } 
        }

        public Usuario() { }
        public Usuario(string nome, string email)
        {
            Nome = nome;
            Email = email;
            CriadoEm = DateTime.UtcNow;
        }

        public void Update(Usuario usuario)
        {
            this.Nome = usuario.Nome;
            this.Email = usuario.Email;
        }
    }
}