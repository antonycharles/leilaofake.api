using System;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Token;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public LoginController(
            ILogger<LoginController> logger,
            IUsuarioService usuarioService,
            ITokenService tokenService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Autenticação do usuário.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o login.</param>
        /// <returns>Login com informações do usuário e token</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> Authenticate([FromBody]LoginRequest model)
        {
            try
            {
                Usuario user = await _usuarioService.GetByEmailAsync(model.Email);

                // Verifica se o usuário existe
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos!" });

                var token = _tokenService.GenerateToken(user);

                return Ok(token);
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        /// <summary>
        /// Cadastrar um usuário.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o usuário.</param>
        /// <returns>Login com informações do usuário e token</returns>
        [HttpPost("cadastro")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] UsuarioIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioService.InsertAsync(model.ToUsuario());

                    var token = _tokenService.GenerateToken(usuario);

                    return Ok(token);
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch(Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }
    }
}