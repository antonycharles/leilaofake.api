using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Requests
{
    public class LanceIncluirRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leilão Id é obrigatório")]
        public string LeilaoId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor é obrigatório")]
        [Range(1,99999999,ErrorMessage = "Valor do lance mínimo deve ser de {1} até {2}")]
        public double? Valor { get; set; }

        public Lance ToLance(string interessadoId)
        {
            return new Lance(interessadoId, Valor.Value, LeilaoId);
        }
    }
}
