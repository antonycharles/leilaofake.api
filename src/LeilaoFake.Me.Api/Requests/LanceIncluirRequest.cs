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
        public double Valor { get; set; }

        public Lance ToLance(string interessadoId)
        {
            return new Lance(interessadoId, Valor, LeilaoId);
        }
    }
}
