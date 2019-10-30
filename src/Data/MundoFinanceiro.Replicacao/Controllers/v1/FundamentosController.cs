using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Replicacao.Dtos;
using MundoFinanceiro.Shared.Constants;
using MundoFinanceiro.Shared.Dtos;

namespace MundoFinanceiro.Replicacao.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class FundamentosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FundamentosController> _logger;

        public FundamentosController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FundamentosController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarFundamentoAsync(FundamentoDto fundamentoDto)
        {
            try
            {
                var fundamento = _mapper.Map<Fundamento>(fundamentoDto);
                
                _unitOfWork.Fundamentos.Add(fundamento);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"Replicou com sucesso o fundamento (id {fundamentoDto.Id})");
                fundamentoDto = _mapper.Map<FundamentoDto>(fundamento);
                
                return Ok(fundamentoDto);
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseDto("Ocorreu um erro interno na replicação.", e.Message));
            }
        }
    }
}