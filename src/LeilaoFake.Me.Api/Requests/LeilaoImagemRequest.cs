using System.ComponentModel.DataAnnotations;
using LeilaoFake.Me.Api.ValidationAttributeCustom;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Http;

namespace LeilaoFake.Me.Api.Requests
{
    public class LeilaoImagemRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leilão Id é obrigatório")]
        public string LeilaoId { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1000000)] //2000000 - 2mb
        [AllowedExtensions(new string[] {".jpg", ".png"})] 
        public IFormFile Imagem { get; set; }

        public LeilaoImagem ToLeilaoImagem(string leiloadoPorId, string arquivo)
        {
            return new LeilaoImagem(leiloadoPorId,LeilaoId,arquivo);
        }
    }
}