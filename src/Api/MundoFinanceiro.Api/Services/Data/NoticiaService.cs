using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoreLinq;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Api.Exceptions;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Api.Services.Data
{
    [MappedService]
    public class NoticiaService : INoticiaService
    {
        private const string ProviderUrl = "https://newsapi.org/v2/everything?q={0}&apiKey=2aaa77be14d6496885ce7bb053b920cd";
        
        private readonly IRequestProvider _requestProvider;
        private readonly ILogger<HistoricoService> _logger;

        public NoticiaService(IRequestProvider requestProvider, ILogger<HistoricoService> logger)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<NoticiaDto>> BuscarNoticiasAsync(string assunto)
        {
            _logger.LogDebug($"Requisição para buscar notícias do assunto \"{assunto}\"");

            var noticiasResponse = await _requestProvider.GetAsync<NoticiaResponseDto>(string.Format(ProviderUrl, assunto));
            if (!string.Equals(noticiasResponse.Status, "ok", StringComparison.CurrentCultureIgnoreCase))
                throw new AppException("Não foi possível buscar as notícias do papel.");

            return noticiasResponse.Articles
                .DistinctBy(x => x.Title.Trim().ToLower())
                .ToList();
        }
    }
}