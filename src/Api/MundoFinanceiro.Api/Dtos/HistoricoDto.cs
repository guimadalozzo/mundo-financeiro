namespace MundoFinanceiro.Api.Dtos
{
    public class HistoricoDto
    {
        public string Date { get; set; }
        public decimal Open { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }
}