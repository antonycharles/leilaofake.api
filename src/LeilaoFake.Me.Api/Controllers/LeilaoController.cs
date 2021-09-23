using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Infra.Data.Repositories;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LeilaoController : ControllerBase
    {
        private readonly ILeilaoRepository _leilaoRepository;

        public LeilaoController(ILeilaoRepository leilaoRepository)
        {
            _leilaoRepository = leilaoRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Leilao>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> ListaDeLeiloesAsync()
        {
            try
            {
                var listas = await _leilaoRepository.GetAllAsync(new LeilaoPaginacao());
                return Ok(listas);
            }
            catch (Exception e)
            {
                return StatusCode(400, ErrorResponse.From(e));
            }
        }

        [HttpGet("{leilaoId}")]
        [ProducesResponseType(typeof(Leilao), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> LeilaoAsync(string leilaoId)
        {
            try
            {
                var leilao = await _leilaoRepository.GetByIdAsync(leilaoId);

                if (leilao == null)
                    throw new ArgumentException("Leilão não encontrado!");

                return Ok(leilao);

            }
            catch(Exception e)
            {
                return StatusCode(400, ErrorResponse.From(e));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Leilao), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] LeilaoIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var leilao = await _leilaoRepository.InsertAsync(model.ToLeilao());
                    return Created(leilao, leilao);
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpPut("{leiloadoPorId}/{leilaoId}/cancelar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CancelarLeilaoAsync(string leiloadoPorId, string leilaoId)
        {
            try
            {
                //await _leilaoRepository.UpdateCancelarAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }
    }
}