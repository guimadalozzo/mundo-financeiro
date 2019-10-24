using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Crawler.Helpers;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Crawler.Services.Data
{
    [MappedService]
    internal class FundamentoCrawler : IFundamentoCrawler
    {
        // Id do parâmetro de número máximo de tasks
        private const short MaximoNumeroThreads = 1;
        
        // Id do parâmetro de intervalo de processamentos
        private const short IntervaloProcessamentos = 2;
        
        private readonly ILogger<FundamentoCrawler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public FundamentoCrawler(ILogger<FundamentoCrawler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        private const string BaseUrl = "https://www.fundamentus.com.br/detalhes.php?papel={0}";
        private const string LPAPath = "//div[contains(@class, 'conteudo')]/table[3]/tr[2]/td[6]/span";
        private const string ROEPath = "//div[contains(@class, 'conteudo')]/table[3]/tr[9]/td[6]/span";
        private const string VPAPath = "//div[contains(@class, 'conteudo')]/table[3]/tr[3]/td[6]/span";
        private const string ROICPath = "//div[contains(@class, 'conteudo')]/table[3]/tr[8]/td[6]/span";
        private const string ValorMercadoPath = "//div[contains(@class, 'conteudo')]/table[2]/tr[1]/td[2]/span";
        
        public async Task<Fundamento> ProcessarAsync(Papel papel)
        {
            try
            {
                // Busca o HtmlDocument do papel
                var url = string.Format(BaseUrl, papel.Nome.Trim().ToUpper());
                var htmlDoc = await HtmlHelper.GetHtmlDocumentAsync(url);
            
                // Busca os dados da página e monta o fundamento
                var fundamento = new Fundamento
                {
                    PapelId =  papel.Id,
                    LPA = ParseDoubleValue(htmlDoc, LPAPath),
                    ROE = ParseDoubleValue(htmlDoc, ROEPath),
                    VPA = ParseDoubleValue(htmlDoc, VPAPath),
                    ROIC = ParseDoubleValue(htmlDoc, ROICPath),
                    ValorMercado = ParseDecimalValue(htmlDoc, ValorMercadoPath)
                };
                
                _logger.LogInformation($"Processado com sucesso o papel {papel.Nome}.");
                
                // Retorna o fundamento processado
                return fundamento;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        
        // ReSharper disable once AccessToDisposedClosure
        public async Task<ICollection<Fundamento>> ProcessarAsync(IList<Papel> papeis)
        {
            // Instancia a lista de fundamentos que será retornada
            var fundamentos = new List<Fundamento>();
            
            // Define o número máximo de tasks que podem rodar simultaneamente para evitar bloqueio de IP
            var maximoNumeroThreads = await _unitOfWork.Parametros.BuscarValorInteiroAsync(MaximoNumeroThreads, 4);
            _logger.LogInformation($"Número máximo de threads configurado pelo banco: {maximoNumeroThreads}.");

            var intervaloProcessamento = await _unitOfWork.Parametros.BuscarValorInteiroAsync(IntervaloProcessamentos, 10);
            _logger.LogInformation($"Intervalo de processamento configurado pelo banco: {intervaloProcessamento}.");
            
            using (var semaforo  = new SemaphoreSlim(maximoNumeroThreads))
            {
                var tasks = new List<Task>(papeis.Count);
                foreach (var papel in papeis)
                {
                    // Verifica se o semáforo está disponível
                    await semaforo.WaitAsync();
                    _logger.LogDebug($"Papel {papel.Nome} entrou no semáforo.");
                    
                    // Cria a task de processamento dos dados
                    var task = Task.Run(async () => await ProcessarAsync(papel, fundamentos, semaforo, intervaloProcessamento));

                    // Adiciona o processo do papel na lista
                    tasks.Add(task);
                }
                
                // Aguarda o término de todas as tasks
                await Task.WhenAll(tasks.ToArray());
            }

            // Retorna os fundamentos processados
            return fundamentos;
        }

        private async Task ProcessarAsync(Papel papel, ICollection<Fundamento> fundamentos, SemaphoreSlim semaforo, int intervaloProcessamento)
        {
            try
            {
                // Processa o fundamento e adiciona na lista
                var fundamento = await ProcessarAsync(papel);
                fundamentos.Add(fundamento);

                // Aguarda cinco segundos entre processamentos para evitar bloqueio de IP
                _logger.LogInformation($"Delay de {intervaloProcessamento} segundos após processar o papel {papel.Id}...");
                await Task.Delay(TimeSpan.FromSeconds(intervaloProcessamento));
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha em processar o papel {papel.Nome}:{e.Message}");
            }
            finally
            {
                semaforo.Release();
                _logger.LogDebug($"Papel {papel.Nome} saiu do semáforo.");
            }
        }

        /// <summary>
        /// Busca o dado no documento pelo caminho passado, valida se encontrou um nodo valido, e, caso encontrado,
        /// já converte o valor para float e arredonda para duas casas decimais. 
        /// </summary>
        /// <param name="htmlDoc">HtmlDocument retornado pela URL informada</param>
        /// <param name="path">Caminho do campo</param>
        /// <returns>Valor do campo convertido</returns>
        private static double ParseDoubleValue(HtmlDocument htmlDoc, string path)
        {
            var node = BuscaNodoHtml(htmlDoc, path);

            // ReSharper disable once PossibleNullReferenceException
            var textValue = LimparInput(HttpUtility.HtmlDecode(node.InnerText));
            if (string.IsNullOrWhiteSpace(textValue))
                textValue = "0";
            
            return Math.Round(double.Parse(textValue), 2);
        }
        
        /// <summary>
        /// Busca o dado no documento pelo caminho passado, valida se encontrou um nodo valido, e, caso encontrado,
        /// já converte o valor para decimal. 
        /// </summary>
        /// <param name="htmlDoc">HtmlDocument retornado pela URL informada</param>
        /// <param name="path">Caminho do campo</param>
        /// <returns>Valor do campo convertido</returns>
        private static decimal ParseDecimalValue(HtmlDocument htmlDoc, string path)
        {
            var node = BuscaNodoHtml(htmlDoc, path);
            
            var textValue = LimparInput(HttpUtility.HtmlDecode(node.InnerText));
            if (string.IsNullOrWhiteSpace(textValue))
                textValue = "0";

            return decimal.Parse(textValue);
        }

        /// <summary>
        /// Busca o nodo Html do caminho passado.
        /// </summary>
        /// <param name="htmlDoc">Documento retornado pelo Html Helper do URL</param>
        /// <param name="path">Caminho do documento</param>
        /// <returns></returns>
        /// <exception cref="NodeNotFoundException">Atira a exceção caso não encontre o nodo no caminho especificado.</exception>
        private static HtmlNode BuscaNodoHtml(HtmlDocument htmlDoc, string path)
        {
            var node = htmlDoc.DocumentNode.SelectSingleNode(path);
            
            if (node == null)
                throw new NodeNotFoundException($"Não foi encontrado o nodo de caminho \"{path}\".");
            
            return node;
        }

        private static string LimparInput(string textInput)
        {
            return textInput
                .Replace(".", "")
                .Replace(",", ".")
                .Replace("\n", "")
                .Replace("%", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
        }
    }
}