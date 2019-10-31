using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Shared.Attributes;
using MundoFinanceiro.Shared.Dtos;
using Newtonsoft.Json;

namespace MundoFinanceiro.Crawler.Services.Data
{
    [MappedService]
    public class ReplicacaoService : IReplicacaoService
    {
        private const string MediaType = "application/json";
        
        private readonly ILogger<ReplicacaoService> _logger;

        public ReplicacaoService(ILogger<ReplicacaoService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async void Replicar<T>(ICollection<string> urls, string endpoint, T objeto)
        {
            if (objeto == null) throw new ArgumentNullException(nameof(objeto));

            foreach (var url in urls)
            {
                var requestUrl = $"{url}/api/{endpoint}";
                await ReplicarAsync(requestUrl, endpoint);
            }
        }

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async void Replicar<T>(ICollection<string> urls, string endpoint, ICollection<T> objetos)
        {
            if (objetos == null) throw new ArgumentNullException(nameof(objetos));
            
            const int maximoNumeroThreads = 6;
            const int intervaloProcessamento = 5;

            using var semaforo = new SemaphoreSlim(maximoNumeroThreads);
            var tasks = new List<Task>(objetos.Count);
            
            foreach (var objeto in objetos)
            {
                // Verifica se o semáforo está disponível
                await semaforo.WaitAsync();
                    
                // Cria a task de processamento dos dados
                var task = Task.Run(async () => await ReplicarAsync(urls, endpoint, objeto, semaforo, intervaloProcessamento)); 
                    
                // Adiciona o processo do papel na lista
                tasks.Add(task);
            }

            // Aguarda o término de todas as tasks
            await Task.WhenAll(tasks.ToArray());
        }

        private async Task ReplicarAsync<T>(ICollection<string> urls, string endpoint, T objeto, SemaphoreSlim semaforo, int intervaloProcessamento)
        {
            try
            {
                foreach (var url in urls)
                {
                    var requestUrl = $"{url}/api/{endpoint}";
                    await ReplicarAsync(requestUrl, objeto);
                }

                await Task.Delay(intervaloProcessamento);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                semaforo.Release();
            }
        }
        
        private async Task ReplicarAsync<T>(string url, T objeto)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);

                using var client = CreateHttpClient();
                
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode) return;
                    
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(JsonConvert.DeserializeObject<ResponseDto>(responseContent).Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        
        private static HttpClient CreateHttpClient()
        {
            var clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
            var httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

            return httpClient;
        }
    }
}