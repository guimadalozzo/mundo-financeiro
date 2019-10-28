using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Crawler.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Shared.Constants;
using MundoFinanceiro.Shared.Dtos;

namespace MundoFinanceiro.Crawler.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class FundamentosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FundamentosController> _logger;
        private readonly IFundamentoService _fundamentoService;

        public FundamentosController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FundamentosController> logger, IFundamentoService fundamentoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fundamentoService = fundamentoService;
        }

        /// <summary>
        /// Processa o fundamento de um papel disponível no sistema. Caso ele já possua um fundamento
        /// processado no dia, retorna o existente.
        /// </summary>
        /// <param name="id">Id do papel</param>
        /// <returns>Fundamento processado do papel</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ProcessarAsync(short id)
        {
            _logger.LogInformation($"Requisição para processar o papel {id}.");
            if (!(await _unitOfWork.Papeis.GetAsync(id) is { } papel))
            {
                _logger.LogError($"Não encontrado papel de id {id}.");
                return BadRequest(new ResponseDto($"O papel informado não foi encontrado."));
            }

            if (!(await _unitOfWork.Fundamentos.BuscaFundamentoDiaAsync(id) is { } fundamentoDia))
            {
                try
                {
                    _logger.LogDebug($"Vai processar o papel {papel.Nome} de id {id}.");
                    fundamentoDia = await _fundamentoService.ProcessarAsync(papel);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro de processamento para o papel {papel.Nome} de id {id}. Mensagem: {e.Message}");
                    return BadRequest(new ResponseDto($"Não foi possível processar o papel {papel.Nome} de id {id}."));
                }
            }

            var fundamentoDto = _mapper.Map<FundamentoDto>(fundamentoDia);
            
            return Ok(fundamentoDto);
        }

        /// <summary>
        /// Processa todos os fundamentos que ainda não foram processados hoje.
        /// </summary>
        /// <returns>Fundamentos processados</returns>
        [HttpGet("Pendentes")]
        public async Task<IActionResult> ProcessarPendentesAsync()
        {
            _logger.LogInformation("Requisição para processar todos os papéis pendentes.");
            
            var papeisPendentes = await _unitOfWork.Papeis.BuscaPapeisPendentesAsync();
            _logger.LogInformation($"Total de papéis pendentes: {papeisPendentes.Count()}.");

            if (papeisPendentes.Count > 0)
            {
                var fundamentosProcessados = await _fundamentoService.ProcessarAsync(papeisPendentes);
                _logger.LogInformation($"Total de fundamentos processados: {fundamentosProcessados.Count()}.");

                var fundamentosDtos = _mapper.Map<IEnumerable<FundamentoDto>>(fundamentosProcessados);
                return Ok(fundamentosDtos);   
            }

            return Ok(new ResponseDto("Não existe papel pendente no momento."));
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork?.Dispose();
            _fundamentoService?.Dispose();
            base.Dispose(disposing);
        }
    }
}