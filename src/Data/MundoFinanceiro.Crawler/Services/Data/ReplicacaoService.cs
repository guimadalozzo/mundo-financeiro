using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
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

        public async void ReplicarFundamento(Fundamento fundamento)
        {
            if (fundamento == null) throw new ArgumentNullException(nameof(fundamento));
            await ReplicarFundamentoAsync(fundamento);
        }
        
        public async void ReplicarFundamentos(ICollection<Fundamento> fundamentos)
        {
            if (fundamentos == null) throw new ArgumentNullException(nameof(fundamentos));
            _logger.LogInformation($"Recebido {fundamentos.Count} fundamento(s) para replicar.");

            // Define o número máximo de tasks que podem rodar simultaneamente 
            var maximoNumeroThreads = await _unitOfWork.Parametros.BuscarValorInteiroAsync(Parametro.MaximoNumeroThreads, 4);
            _logger.LogInformation($"Número máximo de threads configurado pelo banco: {maximoNumeroThreads}.");

            var intervaloProcessamento = await _unitOfWork.Parametros.BuscarValorInteiroAsync(Parametro.IntervaloProcessamentos, 10);
            _logger.LogInformation($"Intervalo de processamento configurado pelo banco: {intervaloProcessamento}.");

            using (var semaforo = new SemaphoreSlim(maximoNumeroThreads))
            {
                var tasks = new List<Task>(fundamentos.Count);
                foreach (var fundamento in fundamentos)
                {
                    // Verifica se o semáforo está disponível
                    await semaforo.WaitAsync();
                    _logger.LogDebug($"Fundamento do papel {fundamento.PapelId} entrou no semáforo.");
                    
                    // Cria a task de processamento dos dados
                    var task = Task.Run(async () => await ReplicarFundamentoAsync(fundamento, semaforo, intervaloProcessamento)); 
                    
                    // Adiciona o processo do papel na lista
                    tasks.Add(task);
                }

                // Aguarda o término de todas as tasks
                await Task.WhenAll(tasks.ToArray());
            }
            
            foreach (var fundamento in fundamentos)
                ReplicarFundamento(fundamento);
        }

        private async Task ReplicarFundamentoAsync(Fundamento fundamento, SemaphoreSlim semaforo, int intervalo)
        {
            try
            {
                await ReplicarFundamentoAsync(fundamento);
                
                // Aguarda X segundos entre processamentos
                _logger.LogInformation($"Delay de {intervalo} segundos após processar o fundamento do papel de id {fundamento.PapelId}...");
                await Task.Delay(TimeSpan.FromSeconds(intervalo));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                semaforo.Release();
                _logger.LogDebug($"Fundamento do papel de id {fundamento.PapelId} saiu do semáforo.");
            }
        }

        private async Task ReplicarFundamentoAsync(Fundamento fundamento)
        {
            // Busca os nodos de replicação ativos
            var nodosReplicacao = await _unitOfWork.Replicacoes.Find(x => x.Ativo).ToListAsync();
            
            // Replica o fundamento em cada um dos nodos enviando uma requisição HTTP
            foreach (var nodo in nodosReplicacao)
            {
                var fundamentoDto = _mapper.Map<FundamentoDto>(fundamento);
                await ReplicarFundamentoAsync(nodo.Url, fundamentoDto);
            }
        }
        
        private async Task ReplicarFundamentoAsync(string url, FundamentoDto fundamento)
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