using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Requests
{
    public class LeilaoIncluirRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Título do leilão obrigatório")]
        [MaxLength(200, ErrorMessage = "Título não pode ser superior a 200 caracteres")]
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lance mínimo obrigatório")]
        public double LanceMinimo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data início do leilão é obrigatório")]
        public DateTime DataInicio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data fim do leilão é obrigatório")]
        public DateTime DataFim { get; set; }

        public Leilao ToLeilao(string leiloadoPorId)
        {
            return new Leilao(leiloadoPorId, this.Titulo, this.Descricao, this.DataInicio, this.DataFim, this.LanceMinimo);
        }
    }
}
