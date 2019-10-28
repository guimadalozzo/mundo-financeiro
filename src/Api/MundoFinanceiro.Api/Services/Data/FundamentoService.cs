using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Api.Models;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Api.Services.Data
{
    [MappedService]
    public class FundamentoService : IFundamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApiSettings _apiSettings;

        public FundamentoService(IUnitOfWork unitOfWork, IOptions<ApiSettings> apiSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));

            if (string.IsNullOrWhiteSpace(_apiSettings.CrawlerUrl))
                throw new ArgumentNullException(nameof(ApiSettings.CrawlerUrl), "A URL do crawler não pode ser nula.");
        }

        public Task<FundamentoDto> BuscarFundamentoAsync(short id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}