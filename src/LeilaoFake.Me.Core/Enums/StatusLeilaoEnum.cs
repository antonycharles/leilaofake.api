using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeilaoFake.Me.Core.Enums
{
    public enum StatusLeilaoEnum
    {
        [Display(Name = "Espera")]
        Espera = 1,

        [Display(Name = "Em andamento")]
        EmAndamento = 2,

        [Display(Name = "Cancelado")]
        Cancelado = 3,

        [Display(Name = "Finalizado")]
        Finalizado = 4
    }
}
