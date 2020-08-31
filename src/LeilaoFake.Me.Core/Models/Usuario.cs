using System;

namespace LeilaoFake.Me.Core.Models
{
    public class Usuario
    {
        public string _id;
        public string Id {
            get
            {
                return _id;
            }

            private set
            {
                if (value == null || value.Length > 20)
                    throw new ArgumentNullException("Usuário id deve ser preenchido corretamente!");

                _id = value;
            }
        }

        public string Nome { get; private set; }

        public Usuario() { }
        public Usuario(string id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public void Update(Usuario usuario)
        {
            this.Nome = usuario.Nome;
        }
    }
}