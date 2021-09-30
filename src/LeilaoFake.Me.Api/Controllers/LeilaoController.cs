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
using Microsoft.Extensions.Logging;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeilaoController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILeilaoService _leilaoService;
        private readonly IUrlHelper _urlHelper;

        public LeilaoController(ILeilaoService leilaoService, IUrlHelper urlHelper, ILogger<LeilaoController> logger)
        {
            _leilaoService = leilaoService;
            _urlHelper = urlHelper;
            _logger = logger;
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
                _logger.LogInformation("Inicio {0}", nameof(GetPaginacaoAsync));
                var usuarioAutenticado = new UsuarioAutenticado(User);
                var listas = await _leilaoService.GetAllAsync(new LeilaoPaginacao(
                    porPagina:porPagina,
                    pagina:pagina,
                    order:order,
                    search:search
                ));
                
                var leilaoPaginacaoResponse = new LeilaoPaginacaoResponse(listas, _urlHelper, usuarioAutenticado);
                leilaoPaginacaoResponse.AddLinkMeusLeiloes();
                leilaoPaginacaoResponse.AddLinkPaginaAnterior();
                leilaoPaginacaoResponse.AddLinkProximaPagina();

                return Ok(leilaoPaginacaoResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Erro"  + nameof(GetPaginacaoAsync));
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

                var leilaoPaginacaoResponse = new LeilaoPaginacaoResponse(listas, _urlHelper, usuarioAutenticado);
                leilaoPaginacaoResponse.AddLinkTodosLeiloes();
                leilaoPaginacaoResponse.AddLinkPaginaAnteriorUsuarioLogado();
                leilaoPaginacaoResponse.AddLinkProximaPaginaUsuarioLogado();
                
                return Ok(leilaoPaginacaoResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Erro"  + nameof(GetAllMeusLeiloesAsync));
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
                var usuarioAutenticado = new UsuarioAutenticado(User);
                var leilao = await _leilaoService.GetByIdAsync(leilaoId);

                if (leilao == null)
                    throw new ArgumentException("Leilão não encontrado!");

                var leilaoResponse = new LeilaoResponse(leilao, _urlHelper, usuarioAutenticado);
                leilaoResponse.AddAllLinks();

                return Ok(leilaoResponse);

            }
            catch(Exception e)
            {
                _logger.LogError(e,"Erro"  + nameof(GetIdAsync));
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

                    var leilaoResponse = new LeilaoResponse(leilao, _urlHelper, usuarioAutenticado);
                    leilaoResponse.AddAllLinks();

                    return CreatedAtAction("GetId", new { leilaoId = leilao.Id }, leilaoResponse);
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                _logger.LogError(e,"Erro"  + nameof(IncluirAsync));
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
                _logger.LogError(e,"Erro"  + nameof(UpdateAsync));
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
                _logger.LogError(e,"Erro"  + nameof(DeleteAsync));
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
                _logger.LogError(e,"Erro"  + nameof(IniciarPregaoAsync));
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
                _logger.LogError(e,"Erro"  + nameof(CancelarAsync));
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
                _logger.LogError(e,"Erro"  + nameof(GetPaginacaoAsync));
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
                _logger.LogError(e,"Erro"  + nameof(GetPaginacaoAsync));
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
                _logger.LogError(e,"Erro"  + nameof(GetPaginacaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }
    }
}