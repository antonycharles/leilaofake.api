using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public interface ILanceRepository
    {
        Task<Lance> GetByIdAsync(string lanceId);
        Task<IList<Lance>> GetAllByLeilaoIdAsync(string leilaoId);
        Task<string> InsertAsync(Lance lance);
    }
}
