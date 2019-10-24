namespace MundoFinanceiro.Crawler.Dtos
{
    public class FundamentoDto
    {
        public int Id { get; set; }
        public short PapelId { get; set; }
        public double LPA { get; set; }
        public double VPA { get; set; }
        public double ROE { get; set; }
        public double ROIC { get; set; }
        public decimal ValorMercado { get; set; }
    }
}