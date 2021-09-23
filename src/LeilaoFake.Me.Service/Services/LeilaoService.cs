using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;

namespace LeilaoFake.Me.Service.Services
{
    public class LeilaoService : ILeilaoService
    {
        private readonly ILeilaoRepository _leilaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public LeilaoService(
            ILeilaoRepository leilaoRepository,
            IUsuarioRepository usuarioRepository)
        {
            _leilaoRepository = leilaoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public Task<IList<Leilao>> GetAllByEmAndamentoAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Leilao> GetByIdAsync(string leilaoId)
        {
            return await _leilaoRepository.GetByIdAsync(leilaoId);
        }

        public async Task<Leilao> InsertAsync(Leilao leilao)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(leilao.LeiloadoPorId);

            if (usuario.Id != leilao.LeiloadoPorId)
                throw new ArgumentException("Usuário informado é inválido!");


            string leilaoId =  await _leilaoRepository.InsertAsync(leilao);

            return await _leilaoRepository.GetByIdAsync(leilaoId);
        }

       public async Task UpdateCancelarAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await _leilaoRepository.GetByIdAsync(leilaoId);

            if (leilao.Id == null || leilao.LeiloadoPorId != leiloadoPorId)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.CancelarLeilao();

            //await _leilaoRepository.u
        }

        public async Task UpdateFinalizarAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetByIdAsync(leilaoId);

            if (leilao.Id == null || leilao.LeiloadoPorId != leiloadoPorId)
                throw new ArgumentException("Leilão não encontrado!");

            leilao.FinalizarLeilao();

            string sql = "UPDATE leiloes SET Status = @Status, LanceGanhadorId = @LanceGanhadorId WHERE Id = @Id";

            //var resultado = await _dbConnection.ExecuteAsync(sql, leilao);

            //if (resultado == 0)
                throw new ArgumentException("Leilão não foi alterado!");
        }
    }
}