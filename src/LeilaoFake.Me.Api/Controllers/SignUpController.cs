using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using LeilaoFake.Me.Service.Services;
using LeilaoFake.Me.Api.Token;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignUpController : ControllerBase
    {
        
        private readonly ILogger<SignUpController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public SignUpController(
            ILogger<SignUpController> logger,
            IUsuarioService usuarioService,
            ITokenService tokenService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Cadastrar um usuário.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o usuário.</param>
        /// <returns>Login com informações do usuário e token</returns>
        [HttpPost]
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