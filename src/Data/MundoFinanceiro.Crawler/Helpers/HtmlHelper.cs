using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MundoFinanceiro.Crawler.Helpers
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Busca o HtmlDocument para o uma URL informada
        /// </summary>
        /// <param name="url">URL para buscar o HtmlDocument</param>
        /// <returns></returns>
        public static async Task<HtmlDocument> GetHtmlDocumentAsync(string url)
        {
            string html;

            using (var client = new HttpClient())
                html = await client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }
    }
}