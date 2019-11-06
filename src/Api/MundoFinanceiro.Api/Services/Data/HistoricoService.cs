using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Api.Services.Data
{
    [MappedService]
    public class HistoricoService : IHistoricoService
    {
        private const string ProviderUrl = "http://www.onvest.com.br/historicos/UPF/SD/getHistorico/{0}/json";
        
        private readonly IRequestProvider _requestProvider;
        private readonly ILogger<HistoricoService> _logger;

        public HistoricoService(IRequestProvider requestProvider, ILogger<HistoricoService> logger)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<HistoricoDto>> BuscarHistoricoPapelAsync(string papel)
        {
            _logger.LogDebug($"Requisição para buscar o histórico do papel {papel}.");

            // Busca o histórico do papel informado
            return await _requestProvider.GetAsync<ICollection<HistoricoDto>>(string.Format(ProviderUrl, papel));
        }
    }
}