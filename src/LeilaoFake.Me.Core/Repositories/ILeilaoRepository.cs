using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Core.Repositories
{
    public interface ILeilaoRepository
    {
        Task<IList<Leilao>> GetLeiloesAllByEmAndamentoAsync();
        Task<IList<Leilao>> GetLeiloesAllByLeiloadoPorIdAsync(string leiloadoPorId);
        Task<Leilao> GetLeilaoByIdAsync(string leilaoId);
        Task<Leilao> InsertLeilaoAsync(Leilao leilao);
        Task UpdateCancelarLeilaoAsync(string leiloadoPorId, string leilaoId);
        Task UpdateFinalizarLeilaoAsync(string leiloadoPorId, string leilaoId);
    }
}
