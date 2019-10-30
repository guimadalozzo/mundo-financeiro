using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Crawler.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Shared.Attributes;
using MundoFinanceiro.Shared.Dtos;
using Newtonsoft.Json;

namespace MundoFinanceiro.Crawler.Services.Data
{
    [MappedService]
    internal class ReplicacaoService : IReplicacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReplicacaoService> _logger;
        private readonly IMapper _mapper;

        public ReplicacaoService(IUnitOfWork unitOfWork, ILogger<ReplicacaoService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void ReplicarFundamentos(ICollection<Fundamento> fundamentos)
        {
            if (fundamentos == null) throw new ArgumentNullException(nameof(fundamentos));
            _logger.LogInformation($"Recebido {fundamentos.Count} fundamento(s) para replicar.");

            foreach (var fundamento in fundamentos)
                ReplicarFundamento(fundamento);
        }
        
        public async void ReplicarFundamento(Fundamento fundamento)
        {
            if (fundamento == null) throw new ArgumentNullException(nameof(fundamento));
            
            // Busca os nodos de replicação ativos
            var nodosReplicacao = await _unitOfWork.Replicacoes.Find(x => x.Ativo).ToListAsync();
            
            // Replica o fundamento em cada um dos nodos enviando uma requisição HTTP
            foreach (var nodo in nodosReplicacao)
            {
                var fundamentoDto = _mapper.Map<FundamentoDto>(fundamento);
                ReplicarFundamento(nodo.Url, fundamentoDto);
            }
        }
        
        private async void ReplicarFundamento(string url, FundamentoDto fundamento)
        {
            try
            {
                if (fundamento == null) 
                    _logger.LogError("O fundamento replicado não pode ser nulo.");
                else
                {
                    var requestUrl = $"{url}/api/v1/Fundamentos";
                    var content = new StringContent(JsonConvert.SerializeObject(fundamento), Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using var client = CreateHttpClient();
                    
                    var response = await client.PostAsync(requestUrl, content);
                    if (response.IsSuccessStatusCode) return;
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
                    throw new Exception(responseDto.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro durante a replicação do fundamento {fundamento?.Id ?? -1} na URL {url}: {e.Message}");
            }
        }
        
        private static HttpClient CreateHttpClient()
        {
            var clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
            var httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}