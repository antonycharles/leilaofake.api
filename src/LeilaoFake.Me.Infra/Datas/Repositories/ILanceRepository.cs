using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Data.Repositories
{
    public interface ILanceRepository
    {
        Task<Lance> GetById(string lanceId);
        Task<IList<Lance>> GetAllByLeilaoId(string leilaoId);
        Task<Lance> InsertAsync(Lance lance);
    }
}
