using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.FormsBodys;
using LeilaoFake.Me.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeilaoFake.Me.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanceController : ControllerBase
    {
        private readonly ILanceRepository _lanceRepository;

        public LanceController(ILanceRepository lanceRepository)
        {
            _lanceRepository = lanceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirAsync([FromBody] LanceIncluirFormBody model)
        {
            try
            {
                var lance = await _lanceRepository.InsertLanceAsync(model.ToLance());
                return Created(lance.Id, lance);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}