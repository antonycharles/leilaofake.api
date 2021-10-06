using System;
using System.Linq;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using LeilaoFake.Me.Service.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanceController : ControllerBase
    {
        private readonly ILanceService _lanceService;
        private readonly IUrlHelper _urlHelper;

        public LanceController(ILanceService lanceService, IUrlHelper urlHelper)
        {
            _lanceService = lanceService;
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Cadastra um novo lance para uma leilão em andamento.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o lance</param>
        /// <returns>Lance criado</returns>
        [HttpPost]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(typeof(LanceResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] LanceIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioAutenticado = new UsuarioAutenticado(User);
                    var lance = await _lanceService.InsertAsync(model.ToLance(usuarioAutenticado.Id));
                    return Created(lance.Id, new LanceResponse(lance,_urlHelper,usuarioAutenticado));
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }
    }
}