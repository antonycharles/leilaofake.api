using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Service.Services
{
    public interface ILanceService
    {
        Task<Lance> GetById(string lanceId);
        Task<IList<Lance>> GetAllByLeilaoId(string leilaoId);
        Task<Lance> InsertAsync(Lance lance);
    }
}