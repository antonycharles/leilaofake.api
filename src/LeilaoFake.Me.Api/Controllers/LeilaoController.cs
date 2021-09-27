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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [AllowAnonymous]
        [ProducesResponseType(typeof(IList<Leilao>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetPaginacaoAsync(int? pagina, int? porPagina, string order, string search)
        {
            try
            {   
                var listas = await _leilaoService.GetAllAsync(new LeilaoPaginacao(
                    porPagina:porPagina,
                    pagina:pagina,
                    order:order,
                    search:search
                ));

                
                return Ok(new LeilaoPaginacaoResponse(listas, _urlHelper));
            }
            catch (Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpGet("meus-leiloes")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(typeof(IList<Leilao>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetAllMeusLeiloesAsync(int? pagina, int? porPagina, string order, string search)
        {
            try
            {   
                var usuarioAutenticado = new UsuarioAutenticado(User);
                var listas = await _leilaoService.GetAllAsync(new LeilaoPaginacao(
                    porPagina:porPagina,
                    pagina:pagina,
                    order:order,
                    search:search,
                    leiloadoPorId: usuarioAutenticado.Id
                ));
                
                return Ok(new LeilaoPaginacaoResponse(listas, _urlHelper));
            }
            catch (Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpGet("{leilaoId}")]
        [AllowAnonymous]
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
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(typeof(Leilao), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] LeilaoIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioAutenticado = new UsuarioAutenticado(User);
                    var leilao = await _leilaoService.InsertAsync(model.ToLeilao(usuarioAutenticado.Id));
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
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateAsync(string leilaoId, [FromBody] LeilaoUpdateRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioAutenticado = new UsuarioAutenticado(User);
                    await _leilaoService.UpdateAsync(model.ToLeilaoUpdate(leilaoId, usuarioAutenticado.Id));
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
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.DeleteAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();

            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/iniciar_pregao")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IniciarPregaoAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.UpdateIniciaPregaoAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/cancelar")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CancelarAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.UpdateCancelarAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/finaliza")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> FinalizarAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.UpdateFinalizarAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/tornar_publico")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> TornarPublicoAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.UpdateTornarPublicoAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }

        [HttpPatch("{leilaoId}/tornar_privado")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> TornarPrivadoAsync(string leilaoId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoService.UpdateTornarPrivadoAsync(usuarioAutenticado.Id, leilaoId);
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound(ErrorResponse.From(e));
            }
        }
    }
}