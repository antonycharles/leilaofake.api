using LeilaoFake.Me.Core.Enums;
using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Requests
{
    public class LeilaoUpdateRequest
    {
        [MaxLength(200, ErrorMessage = "Título não pode ser superior a 200 caracteres")]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public double? LanceMinimo { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public LeilaoUpdate ToLeilaoUpdate(string leilaoId, string leiloadoPorId)
        {
            return new LeilaoUpdate(leilaoId, leiloadoPorId, this.Titulo, this.Descricao, this.LanceMinimo, this.DataInicio, this.DataFim);
        }
    }
}
