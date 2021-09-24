using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Service.Services
{
    public interface ILeilaoService
    {
        Task<LeilaoPaginacao> GetAllAsync(LeilaoPaginacao data);
        Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId);
        Task<Leilao> GetByIdAsync(string leilaoId);
        Task<Leilao> InsertAsync(Leilao leilao);
        Task UpdateAsync(LeilaoUpdate leilaoUpdate);
        Task DeleteAsync(string leiloadoPorId, string leilaoId);
        Task UpdateIniciaPregaoAsync(string leiloadoPorId, string leilaoId);
        Task UpdateCancelarAsync(string leiloadoPorId, string leilaoId);
        Task UpdateFinalizarAsync(string leiloadoPorId, string leilaoId);
        Task UpdateTornarPublicoAsync(string leiloadoPorId, string leilaoId);
        Task UpdateTornarPrivadoAsync(string leiloadoPorId, string leilaoId);
    }
}