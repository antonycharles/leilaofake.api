using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Infra.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace LeilaoFake.Me.Service.Services
{
    public class LeilaoService : ILeilaoService
    {
        private readonly ILeilaoRepository _leilaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger _logger;

        public LeilaoService(
            ILeilaoRepository leilaoRepository,
            IUsuarioRepository usuarioRepository,
            ILogger<LeilaoService> logger)
        {
            _leilaoRepository = leilaoRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<LeilaoPaginacao> GetAllAsync(LeilaoPaginacao data)
        {
            _logger.LogInformation("Dados: " + JsonSerializer.Serialize(data));
            return await _leilaoRepository.GetAllAsync(data);
        }

        public async Task<IList<Leilao>> GetAllByLeiloadoPorIdAsync(string leiloadoPorId)
        {
            return await _leilaoRepository.GetAllByLeiloadoPorIdAsync(leiloadoPorId);
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

        public async Task UpdateAsync(LeilaoUpdate leilaoUpdate)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoUpdate.LeilaoId, leilaoUpdate.LeiloadoPorId);

            leilao.Update(leilaoUpdate);

            await _leilaoRepository.UpdateAsync(leilao);
        }

        public async Task DeleteAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            if(!leilao.IsDelete)
                throw new ArgumentException("Não é possivel deletar este leilão");

            await _leilaoRepository.DeleteAsync(leilao.Id);
        }

        public async Task UpdateIniciaPregaoAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            leilao.IniciaPregao();

            await _leilaoRepository.UpdateAsync(leilao);
        }

       public async Task UpdateCancelarAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            leilao.CancelarLeilao();

            await _leilaoRepository.UpdateAsync(leilao);
        }

        public async Task UpdateFinalizarAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            leilao.FinalizarLeilao();

            await _leilaoRepository.UpdateAsync(leilao);
        }

        public async Task UpdateTornarPublicoAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            leilao.TornarPublico();

            await _leilaoRepository.UpdateAsync(leilao);
        }

        public async Task UpdateTornarPrivadoAsync(string leiloadoPorId, string leilaoId)
        {
            var leilao = await this.GetLeilaoByIdAndLeiloadoPorId(leilaoId, leiloadoPorId);

            leilao.TornarPrivado();

            await _leilaoRepository.UpdateAsync(leilao);
        }

        private async Task<Leilao> GetLeilaoByIdAndLeiloadoPorId(string leilaoId, string leiloadoPorId)
        {
            var leilao = await _leilaoRepository.GetByIdAsync(leilaoId);

            if (leilao == null || leilao.LeiloadoPorId != leiloadoPorId)
                throw new ArgumentException("Leilão não encontrado!");

            return leilao;
        }
    }
}