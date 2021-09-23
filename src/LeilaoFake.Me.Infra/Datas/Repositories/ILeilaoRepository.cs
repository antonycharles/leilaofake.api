using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Data.Repositories
{
    public interface ILeilaoRepository
    {
        Task<LeilaoPaginacao> GetAllAsync(LeilaoPaginacao leilaoPaginacao);
        Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId);
        Task<Leilao> GetByIdAsync(string leilaoId);
        Task<string> InsertAsync(Leilao leilao);
        Task UpdateAsync(Leilao leilao);
        Task DeleteAsync(string leilaoId);
    }
}
