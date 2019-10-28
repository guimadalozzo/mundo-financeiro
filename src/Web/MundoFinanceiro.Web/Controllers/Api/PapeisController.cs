using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Web.Dtos;

namespace MundoFinanceiro.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PapeisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PapeisController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("Disponiveis")]
        public async Task<IActionResult> BuscaPapeisDisponiveis(string query = null)
        {
            var papeisDisponiveis = string.IsNullOrWhiteSpace(query)
                ? await _unitOfWork.Papeis.Find(x => x.Ativo).ToListAsync()
                : await _unitOfWork.Papeis.Find(x => x.Ativo && x.Nome.ToUpper().Contains(query.ToUpper())).ToListAsync();
                
            var papeisDisponiveisDtos = _mapper.Map<IEnumerable<PapelDisponivelDto>>(papeisDisponiveis); 
            
            return Ok(papeisDisponiveisDtos);
        }
    }
}