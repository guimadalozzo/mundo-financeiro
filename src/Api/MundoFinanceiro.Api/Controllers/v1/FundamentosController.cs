using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Exceptions;
using MundoFinanceiro.Shared.Constants;
using MundoFinanceiro.Shared.Dtos;

namespace MundoFinanceiro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class FundamentosController : Controller
    {
        private readonly IFundamentoService _fundamentoService;
        private readonly ILogger<FundamentosController> _logger;

        public FundamentosController(IFundamentoService fundamentoService, ILogger<FundamentosController> logger)
        {
            _fundamentoService = fundamentoService ?? throw new ArgumentNullException(nameof(fundamentoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscaFundamentoAsync(short id)
        {
            try
            {
                _logger.LogDebug($"Buscando fundamentos do papel de id {id}.");
                var fundamentoDto = await _fundamentoService.BuscarFundamentoAsync(id);

                return Ok(fundamentoDto);
            }
            catch (AppException e)
            {
                return BadRequest(new ResponseDto(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseDto("Ocorreu um erro interno no sistema! Tente novamente mais tarde.", e.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            _fundamentoService?.Dispose();
            base.Dispose(disposing);
        }
    }
}