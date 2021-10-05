using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Service.Services
{
    public interface ILeilaoImagemService
    {
        Task<LeilaoImagem> InsertAsync(LeilaoImagem leilaoImagem);
        Task DeleteAsync(string leiloadoPorId, string leilaoId, int leilaoImagemId);
    }
}