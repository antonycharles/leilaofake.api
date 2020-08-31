using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.FormsBodys
{
    public class LeilaoIncluirFormBody
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Título do leilão obrigatório")]
        public string Titulo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leiloador id obrigatório")]
        public string LeiloadoPorId { get; set; }

        public string LeiloadoPorNome { get; set; }

        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lance mínimo obrigatório")]
        public double LanceMinimo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data início do leilão é obrigatório")]
        public DateTime DataInicio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data fim do leilão é obrigatório")]
        public DateTime DataFim { get; set; }

        public Leilao ToLeilao()
        {
            var leiloador = new Usuario(this.LeiloadoPorId, this.LeiloadoPorNome);
            return new Leilao(null, leiloador, this.Titulo, this.DataInicio, this.DataFim, this.LanceMinimo);
        }
    }
}
