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
using Microsoft.AspNetCore.Authorization;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Pesquisa usuários públicos.
        /// </summary>
        /// <param name="pagina"> página atual da pesquisa.</param>
        /// <param name="porPagina"> total de itens por página.</param>
        /// <param name="order"> ordenação da pesquisa.</param>
        /// <returns>Usuários paginação response</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UsuarioPaginacao), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
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

        /// <summary>
        /// Pesquisa usuário.
        /// </summary>
        /// <param name="id"> id do usuário que deseja buscar.</param>
        /// <returns>Leilões response</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UsuarioResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetIdAsync(string id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);

                if(usuario == null)
                    throw new Exception("Usuário não encontrado");

                var usuarioResponse = new UsuarioResponse(usuario);

                return Ok(usuarioResponse);
            }
            catch (Exception e)
            {
                return StatusCode(404, ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o usuário</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UsuarioResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] UsuarioIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioService.InsertAsync(model.ToUsuario());

                    var usuarioResponse = new UsuarioResponse(usuario);

                    return Created(usuario.Id, usuarioResponse);
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Altera usuário.
        /// </summary>
        /// <param name="id">id do usuário que vai ser alterado.</param>
        /// <param name="model"> FromBody com as informações sober o usuário.</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
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

        /// <summary>
        /// Remover usuário.
        /// </summary>
        /// <param name="id">id do usuário que vai ser deletado.</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
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