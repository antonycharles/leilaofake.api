using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.FormsBodys
{
    public class LanceIncluirFormBody
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Interessado Id é obrigatório")]
        public string InteressadoId { get; set; }

        public string InteressadoNome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leilão Id é obrigatório")]
        public string LeilaoId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor é obrigatório")]
        public double Valor { get; set; }

        public Lance ToLance()
        {
            var interredado = new Usuario(this.InteressadoId, this.InteressadoNome);
            return new Lance(null, interredado, this.Valor);
        }
    }
}
