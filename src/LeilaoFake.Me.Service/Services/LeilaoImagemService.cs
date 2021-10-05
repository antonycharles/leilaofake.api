using System;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Datas.Repositories;

namespace LeilaoFake.Me.Service.Services
{
    public class LeilaoImagemService : ILeilaoImagemService
    {
        public readonly ILeilaoImagemRepository _leilaoImagemRepository;
        public readonly ILeilaoRepository _leilaoRepository;
        public readonly IUsuarioRepository _usuarioRepository;

        public LeilaoImagemService(ILeilaoImagemRepository leilaoImagemRepository, ILeilaoRepository leilaoRepository, IUsuarioRepository usuarioRepository)
        {
            _leilaoImagemRepository = leilaoImagemRepository;
            _leilaoRepository = leilaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<LeilaoImagem> InsertAsync(LeilaoImagem leilaoImagem)
        {
            var leilao = await _leilaoRepository.GetByIdAndLeiloadoPorIdAsync(leilaoImagem.LeilaoId, leilaoImagem.LeiloadoPorId);

            if (leilao == null)
                throw new ArgumentException("Leilão não encontrado!");

            if(!leilao.IsUpdate)
                throw new ArgumentException("Não é possível alterar o leilão");
            
            int leilaoImagemId =  await _leilaoImagemRepository.InsertAsync(leilaoImagem);

            return await _leilaoImagemRepository.GetByIdAsync(leilaoImagemId);
        }
        
        public async Task DeleteAsync(string leiloadoPorId, string leilaoId, int leilaoImagemId)
        {
            var leilao = await _leilaoRepository.GetByIdAndLeiloadoPorIdAsync(leilaoId, leiloadoPorId);

            if (leilao == null)
                throw new ArgumentException("Leilão não encontrado!");

            await _leilaoImagemRepository.DeleteAsync(leilaoImagemId);
        }
    }
}