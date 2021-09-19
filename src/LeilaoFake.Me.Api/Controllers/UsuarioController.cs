using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Core.Services;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Usuario>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> ListaDeLeiloesAsync(int? pagina, int? porPagina)
        {
            try
            {
                var usuarioPaginacao = new UsuarioPaginacao(porPagina:porPagina, pagina:pagina);
                var listas = await _usuarioService.GetAllAsync(usuarioPaginacao);
                return Ok(listas);
            }
            catch (Exception e)
            {
                return StatusCode(400, ErrorResponse.From(e));
            }
        }
    }
}