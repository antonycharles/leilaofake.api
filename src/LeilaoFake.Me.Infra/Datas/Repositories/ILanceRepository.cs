using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Data.Repositories
{
    public interface ILanceRepository
    {
        Task<Lance> GetLanceById(string lanceId);
        Task<IList<Lance>> GetLancesByLeilaoId(string leilaoId);
        Task<Lance> InsertLanceAsync(Lance lance);
    }
}
