using System;
using System.IO;
using System.Threading.Tasks;
using LeilaoFake.Me.Api.Requests;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Core.Models;
using LeilaoFake.Me.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LeilaoFake.Me.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeilaoImagemController : ControllerBase
    {

        public readonly ILogger _logger;
        public readonly IWebHostEnvironment _environment;
        public readonly ILeilaoImagemService _leilaoImagemService;
        public readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;

        public LeilaoImagemController(
            IWebHostEnvironment environment,
            ILogger<LeilaoImagemController> logger,
            ILeilaoImagemService leilaoImagemService,
            IConfiguration configuration, IUrlHelper urlHelper)
        {
            _environment = environment;
            _logger = logger;
            _leilaoImagemService = leilaoImagemService;
            _configuration = configuration;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(typeof(Lance), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Incluir([FromForm] LeilaoImagemRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    string arquivo = await this.SalveArquivoAsync(model);
                    var usuarioAutenticado = new UsuarioAutenticado(User);

                    var leilaoImagem = await _leilaoImagemService.InsertAsync(model.ToLeilaoImagem(usuarioAutenticado.Id,arquivo));

                    return Created(leilaoImagem.Url, new LeilaoImagemResponse(leilaoImagem, _urlHelper, usuarioAutenticado));
                }

                return BadRequest(ErrorResponse.FromModelState(ModelState));
            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.From(e));
            }
        }

        [HttpDelete("{leilaoId}/{leilaoImagemId}")]
        [Authorize(Roles = "default,admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> DeleteAsync(string leilaoId, int leilaoImagemId)
        {
            try
            {
                var usuarioAutenticado = new UsuarioAutenticado(User);
                await _leilaoImagemService.DeleteAsync(usuarioAutenticado.Id, leilaoId, leilaoImagemId);
                return Ok();

            }
            catch(Exception e)
            {
                _logger.LogError(e,"Erro"  + nameof(DeleteAsync));
                return NotFound(ErrorResponse.From(e));
            }
        }

        private async Task<string> SalveArquivoAsync(LeilaoImagemRequest model)
        {
            var extensao = model.Imagem.FileName.Split(".");

            string novoNome = Guid.NewGuid().ToString() + "." + extensao[1];
            string diretorio = _configuration.GetValue<string>("DiretorioImagens");
            string novoNomeDiretorio = diretorio + novoNome;

            if (!Directory.Exists(_environment.WebRootPath + diretorio))
                Directory.CreateDirectory(_environment.WebRootPath + diretorio);

            if (System.IO.File.Exists(_environment.WebRootPath + novoNomeDiretorio))
                throw new ArgumentException("Um arquivo com este novo já existe!");

            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + novoNomeDiretorio))
            {
                await model.Imagem.CopyToAsync(filestream);
                filestream.Flush();
            }

            return novoNomeDiretorio;
        }
    }
}