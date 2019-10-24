namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Parametro
    {
        public short Id { get; set; }
        public string Chave { get; set; }
        public string Descricao { get; set; }
        public string ValorTexto { get; set; }
        public decimal? ValorNumerico { get; set; }
    }
}