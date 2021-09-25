using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Service.Services;
using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Security.Policy;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeilaoController : ControllerBase
    {
        private readonly ILeilaoService _leilaoService;
        private readonly IUrlHelper _urlHelper;

        public LeilaoController(ILeilaoService leilaoService, IUrlHelper urlHelper)
        {
            _leilaoService = leilaoService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Leilao>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetPaginacaoAsync(int? pagina, int? porPagina, string order, string search, string leiloadoPorId)
        {
            try
            {   
                var listas = await _leilaoService.GetAllAsync(new LeilaoPaginacao(
                    porPagina:porPagina,
                    pagina:pagina,
                    order:order,
                    search:search,
                    leiloadoPorId: leiloadoPorId
                ));
                
                return Ok(new LeilaoPaginacaoResponse(listas, _urlHelper));
            }
            catch (Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpGet("{leilaoId}")]
        [ProducesResponseType(typeof(Leilao), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetIdAsync(string leilaoId)
        {
            try
            {
                var leilao = await _leilaoService.GetByIdAsync(leilaoId);

                if (leilao == null)
                    throw new ArgumentException("Leilão não encontrado!");

                return Ok(new LeilaoResponse(leilao, _urlHelper));

            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
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
                    var leilao = await _leilaoService.InsertAsync(model.ToLeilao());
                    return CreatedAtAction("GetId", new { leilaoId = leilao.Id }, new LeilaoResponse(leilao, _urlHelper));
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpPut("{leilaoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateAsync(string leilaoId, string leiloadoPorId, [FromBody] LeilaoUpdateRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _leilaoService.UpdateAsync(model.ToLeilaoUpdate(leilaoId, leiloadoPorId));
                    return Ok();
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpDelete("{leilaoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.DeleteAsync(leiloadoPorId, leilaoId);
                return Ok();

            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/iniciar_pregao")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IniciarPregaoAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.UpdateIniciaPregaoAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/cancelar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CancelarAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.UpdateCancelarAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/finaliza")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> FinalizarAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.UpdateFinalizarAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/tornar_publico")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> TornarPublicoAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.UpdateTornarPublicoAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/tornar_privado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> TornarPrivadoAsync(string leilaoId, string leiloadoPorId = null)
        {
            try
            {
                await _leilaoService.UpdateTornarPrivadoAsync(leiloadoPorId, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }
    }
}