using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Data.Repositories
{
    public interface ILeilaoRepository
    {
        Task<IList<Leilao>> GetAllByEmAndamentoAsync();
        Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId);
        Task<Leilao> GetByIdAsync(string leilaoId);
        Task<Leilao> InsertAsync(Leilao leilao);
        Task UpdateCancelarAsync(string leiloadoPorId, string leilaoId);
        Task UpdateFinalizarAsync(string leiloadoPorId, string leilaoId);
    }
}
