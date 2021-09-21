using System.ComponentModel.DataAnnotations;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Api.Requests
{
    public class UsuarioIncluirRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome é obrigatório")]
        [MaxLength(80, ErrorMessage = "Nome não pode ser superior a {1} caracteres")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email é obrigatório")]
        [MaxLength(250, ErrorMessage = "Email não pode ser superior a {1} caracteres")]
        public string Email { get; set; }

        public Usuario ToUsuario()
        {
            return new Usuario(Nome, Email);
        }
    }
}