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

        /// <summary>
        /// Pesquisa leilões públicos.
        /// </summary>
        /// <param name="pagina"> página atual da pesquisa.</param>
        /// <param name="porPagina"> total de itens por página.</param>
        /// <param name="order"> ordenação da pesquisa.</param>
        /// <param name="search"> palavra chave da pesquisa.</param>
        /// <param name="meusLeiloes"> informa se você deseja listar seus leilões.</param>
        /// <returns>Leilões paginação response</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LeilaoPaginacaoResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetPaginacaoAsync(int? pagina, int? porPagina, string order, string search, Boolean? meusLeiloes = false)
        {
            try
            {
                _logger.LogInformation("Inicio {0}", nameof(GetPaginacaoAsync));
                var usuarioAutenticado = new UsuarioAutenticado(User);

                var leilaoPaginacao = new LeilaoPaginacao(
                    porPagina: porPagina,
                    pagina: pagina,
                    order: order,
                    search: search
                );

                if (meusLeiloes == true)
                {
                    if (usuarioAutenticado.IsAuthenticated == false)
                        return Unauthorized();

                    leilaoPaginacao.LeiloadoPorId = usuarioAutenticado.Id;
                }


                var listas = await _leilaoService.GetAllAsync(leilaoPaginacao);

                var leilaoPaginacaoResponse = new LeilaoPaginacaoResponse(listas, _urlHelper, usuarioAutenticado);
                leilaoPaginacaoResponse.AddLinkPaginaAnterior();
                leilaoPaginacaoResponse.AddLinkProximaPagina();

                return Ok(leilaoPaginacaoResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(GetPaginacaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Pesquisa leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja buscar.</param>
        /// <returns>Leilões response</returns>
        [HttpGet("{leilaoId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LeilaoResponse), 200)]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(GetIdAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Cadastra um novo leilão.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o leilão</param>
        /// <returns>Leilão criado</returns>
        [HttpPost]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(typeof(LeilaoResponse), 201)]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(IncluirAsync));
                return BadRequest(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Altera um leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja alterar.</param>
        /// <param name="model"> FromBody com as informações sober o leilão</param>
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(UpdateAsync));
                return BadRequest(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Remove um leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja deletar.</param>
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(DeleteAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Inicia pregão do leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja iniciar pregão.</param>
        [HttpPatch("{leilaoId}/iniciar")]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(IniciarPregaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Cancela leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja cancelar.</param>
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(CancelarAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Finaliza leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja finalizar.</param>
        [HttpPatch("{leilaoId}/finalizar")]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(GetPaginacaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Tornar público leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja tornar público.</param>
        [HttpPatch("{leilaoId}/publico")]
        [Authorize(Roles = "admin")]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(GetPaginacaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Tornar privado leilão.
        /// </summary>
        /// <param name="leilaoId"> id do leilão que deseja tornar privado.</param>
        [HttpPatch("{leilaoId}/privado")]
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
            catch (Exception e)
            {
                _logger.LogError(e, "Erro" + nameof(GetPaginacaoAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }
    }
}