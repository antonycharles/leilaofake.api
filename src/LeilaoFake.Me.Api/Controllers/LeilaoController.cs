using LeilaoFake.Me.Api.FormsBodys;
using LeilaoFake.Me.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeilaoController : ControllerBase
    {
        private readonly ILeilaoRepository _leilaoRepository;

        public LeilaoController(ILeilaoRepository leilaoRepository)
        {
            _leilaoRepository = leilaoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListaDeLeiloesAsync()
        {
            try
            {
                var listas = await _leilaoRepository.GetLeiloesAllByEmAndamentoAsync();
                return Ok(listas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{leilaoId}")]
        public async Task<IActionResult> LeilaoAsync(string leilaoId)
        {
            try
            {
                var leilao = await _leilaoRepository.GetLeilaoByIdAsync(leilaoId);

                if (leilao == null)
                    throw new ArgumentException("Leilão não encontrado!");

                return Ok(leilao);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> IncluirAsync([FromBody] LeilaoIncluirFormBody model)
        {
            try
            {
                var leilao = await _leilaoRepository.InsertLeilaoAsync(model.ToLeilao());
                return Created(leilao.Id, leilao);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{leiloadoPorId}/{leilaoId}/cancelar")]
        public async Task<IActionResult> CancelarLeilaoAsync(string leiloadoPorId, string leilaoId)
        {
            try
            {
                await _leilaoRepository.UpdateCancelarLeilaoAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}