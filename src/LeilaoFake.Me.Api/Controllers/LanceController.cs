using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.ErrorsApi;
using LeilaoFake.Me.Api.ModelsApi;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LanceController : ControllerBase
    {
        private readonly ILanceRepository _lanceRepository;

        public LanceController(ILanceRepository lanceRepository)
        {
            _lanceRepository = lanceRepository;
        }

        /// <summary>
        /// Cadastra um novo lance para uma leilão em andamento.
        /// </summary>
        /// <param name="model"> FromBody com as informações sober o lance</param>
        /// <returns>Lance criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Lance), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> IncluirAsync([FromBody] LanceIncluirFormBody model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lance = await _lanceRepository.InsertLanceAsync(model.ToLance());
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