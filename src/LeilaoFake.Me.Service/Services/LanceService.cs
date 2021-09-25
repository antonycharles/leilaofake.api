using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;

namespace LeilaoFake.Me.Service.Services
{
    public class LanceService : ILanceService
    {
        public readonly ILanceRepository _lanceRepository;
        public readonly ILeilaoRepository _leilaoRepository;
        public readonly IUsuarioRepository _usuarioRepository;

        public LanceService(ILanceRepository lanceRepository, ILeilaoRepository leilaoRepository, IUsuarioRepository usuarioRepository)
        {
            _lanceRepository = lanceRepository;
            _leilaoRepository = leilaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IList<Lance>> GetAllByLeilaoId(string leilaoId)
        {
            return await _lanceRepository.GetAllByLeilaoIdAsync(leilaoId);
        }

        public async Task<Lance> GetById(string lanceId)
        {
            return await _lanceRepository.GetByIdAsync(lanceId);
        }

        public async Task<Lance> InsertAsync(Lance lance)
        {
            var leilao = await _leilaoRepository.GetByIdAsync(lance.LeilaoId);

            if (leilao == null)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.RecebeLance(lance);

            var usuario = await _usuarioRepository.GetByIdAsync(lance.InteressadoId);

            if (usuario == null)
                throw new ArgumentException("Usuário informado é inválido!");
            
            string lanceId =  await _lanceRepository.InsertAsync(lance);

            return await _lanceRepository.GetByIdAsync(lanceId);
        }
    }
}