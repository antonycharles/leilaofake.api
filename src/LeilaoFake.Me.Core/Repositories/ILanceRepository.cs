using LeilaoFake.Me.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Core.Repositories
{
    public interface ILanceRepository
    {
        Task<Lance> GetLanceById(string lanceId);
        Task<IList<Lance>> GetLancesByLeilaoId(string leilaoId);
        Task<Lance> InsertLanceAsync(Lance lance);
    }
}
