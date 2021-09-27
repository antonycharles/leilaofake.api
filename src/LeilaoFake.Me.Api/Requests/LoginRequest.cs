using System.ComponentModel.DataAnnotations;

namespace LeilaoFake.Me.Api.Requests
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email é obrigatório")]
        [MaxLength(250, ErrorMessage = "Email não pode ser superior a {1} caracteres")]
        public string Email { get; set; }
    }
}