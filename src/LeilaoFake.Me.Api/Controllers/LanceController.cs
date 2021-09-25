using System;
using System.Linq;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Core.Models;
using Microsoft.AspNetCore.Mvc;
using LeilaoFake.Me.Service.Services;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanceController : ControllerBase
    {
        private readonly ILanceService _lanceService;

        public LanceController(ILanceService lanceService)
        {
            _lanceService = lanceService;
        }


        /// <summary>
        /// Cadastra um novo lance para uma leilão em andamento.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o lance</param>
        /// <returns>Lance criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Lance), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] LanceIncluirRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lance = await _lanceService.InsertAsync(model.ToLance());
                    return Created(lance.Id, lance);
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