using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Api.Exceptions;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Shared.Constants;
using MundoFinanceiro.Shared.Dtos;

namespace MundoFinanceiro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class PapeisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFundamentoService _fundamentoService;
        private readonly IHistoricoService _historicoService;
        private readonly INoticiaService _noticiaService;
        private readonly ILogger<PapeisController> _logger;
        private readonly IMapper _mapper;

        public PapeisController(IUnitOfWork unitOfWork,
            IFundamentoService fundamentoService,
            IHistoricoService historicoService,
            ILogger<PapeisController> logger,
            IMapper mapper,
            INoticiaService noticiaService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fundamentoService = fundamentoService ?? throw new ArgumentNullException(nameof(fundamentoService));
            _historicoService = historicoService ?? throw new ArgumentNullException(nameof(historicoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _noticiaService = noticiaService ?? throw new ArgumentNullException(nameof(noticiaService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarDadosPapelAsync(short id)
        {
            try
            {
                if (!(await _unitOfWork.Papeis.SingleOrDefaultAsync(x => x.Id == id && x.Ativo) is { } papel))
                    return BadRequest(new ResponseDto($"Não foi possivel encontrar o papel de id {id}."));
                
                _logger.LogDebug($"Buscando dados do papel de id {id}.");
                
                // Busca os fundamentos do papel
                var fundamentos = await _fundamentoService.BuscarFundamentoAsync(id);
                
                // Busca o histórico do papel
                var historico = await _historicoService.BuscarHistoricoPapelAsync(papel.Nome);
                
                // Busca as notícias do papel
                var noticias = await _noticiaService.BuscarNoticiasAsync(papel.Nome); 
                
                // Mapeia o papel para PapelDto
                var papelDto = _mapper.Map<PapelDto>(papel);
                
                return Ok(new DadosPapelDto(papelDto, fundamentos, historico, noticias));
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
            _unitOfWork?.Dispose();
            base.Dispose(disposing);
        }
    }
}