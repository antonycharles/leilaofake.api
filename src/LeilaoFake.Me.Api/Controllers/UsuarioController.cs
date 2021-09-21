using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Service.Services;
using System.Linq;

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
        [ProducesResponseType(typeof(UsuarioPaginacao), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetAllAsync(int? pagina, int? porPagina, string order)
        {
            try
            {

                var usuarioPaginacao = new UsuarioPaginacao(porPagina:porPagina, pagina:pagina, order:order);
                var usuarios = await _usuarioService.GetAllAsync(usuarioPaginacao);
                return Ok(usuarios);
            }
            catch (Exception e)
            {
                return StatusCode(404, ErrorResponse.From(e));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetIdAsync(string id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);

                if(usuario == null)
                    throw new Exception("Usuário não encontrado");

                return Ok(usuario);
            }
            catch (Exception e)
            {
                return StatusCode(404, ErrorResponse.From(e));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] UsuarioIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioService.InsertAsync(model.ToUsuario());
                    return Created(usuario.Id, usuario);
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UsuarioUpdateRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _usuarioService.UpdateAsync(id, model.ToUsuario());
                    return Ok();
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _usuarioService.DeleteAsync(id);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(404, ErrorResponse.From(e));
            }
        }
    }
}