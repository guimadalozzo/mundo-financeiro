namespace MundoFinanceiro.Api.Models
{
    public class ApiSettings
    {
        /// <summary>
        /// Url da API do Crawler usada para buscar os fundamentos e também processar
        /// o fundamento do papel em questão caso necessário.
        /// </summary>
        public string CrawlerUrl { get; set; }
    }
}