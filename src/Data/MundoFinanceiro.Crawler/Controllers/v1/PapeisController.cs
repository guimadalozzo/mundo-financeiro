using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MundoFinanceiro.Crawler.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Shared.Constants;

namespace MundoFinanceiro.Crawler.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class PapeisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public PapeisController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> BuscaPapeisAsync()
        {
            var papeis = await _unitOfWork.Papeis.GetAllAsync();
            var papeisDtos = _mapper.Map<IEnumerable<PapelDto>>(papeis);

            return Ok(papeisDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscaPapelAsync(short id)
        {
            if (!(await _unitOfWork.Papeis.GetAsync(id) is { } papel))
                return BadRequest(new ResponseDto("Papel não encontrado."));
                        
            var papelDto = _mapper.Map<PapelDto>(papel);

            return Ok(papelDto);
        }

        [HttpGet("Pendentes")]
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}