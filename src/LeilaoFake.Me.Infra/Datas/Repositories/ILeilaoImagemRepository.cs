using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Infra.Datas.Repositories
{
    public interface ILeilaoImagemRepository
    {
         
        Task<LeilaoImagem> GetByIdAsync(int leilaoImagemId);
        Task<int> InsertAsync(LeilaoImagem leilaoImagem);
        Task DeleteAsync(int leilaoImagemId);
    }
}