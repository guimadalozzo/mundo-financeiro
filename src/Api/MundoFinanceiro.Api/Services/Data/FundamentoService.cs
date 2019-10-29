using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Api.Exceptions;
using MundoFinanceiro.Api.Models;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Shared.Attributes;
using MundoFinanceiro.Shared.Dtos;
using Newtonsoft.Json;

namespace MundoFinanceiro.Api.Services.Data
{
    [MappedService]
    public class FundamentoService : IFundamentoService
    {
        private const int TimeoutSeconds = 30;
        private const string MediaType = "application/json";

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApiSettings _apiSettings;

        public FundamentoService(IUnitOfWork unitOfWork, IOptions<ApiSettings> apiSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));

            if (string.IsNullOrWhiteSpace(_apiSettings.CrawlerUrl))
                throw new ArgumentNullException(nameof(ApiSettings.CrawlerUrl), "A URL do crawler não pode ser nula.");
        }

        public async Task<FundamentoDto> BuscarFundamentoAsync(short id)
        {
            var requestUrl = $"{_apiSettings.CrawlerUrl}/api/v1/Fundamentos/{id}";

            using var client = CreateHttpClient();
            var response = await client.GetAsync(requestUrl);
            var fundamentoDto = await DeserializarFundamentoAsync(response);

            return fundamentoDto;
        }

        private static HttpClient CreateHttpClient()
        {
            var clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
            var httpClient = new HttpClient(clientHandler) { Timeout = TimeSpan.FromSeconds(TimeoutSeconds) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

            return httpClient;
        }
        
        private static async Task<FundamentoDto> DeserializarFundamentoAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<FundamentoDto>(content);
            
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            throw new AppException(responseDto.Message);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}