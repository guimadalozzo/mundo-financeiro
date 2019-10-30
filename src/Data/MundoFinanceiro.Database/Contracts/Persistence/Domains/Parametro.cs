namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Parametro
    {
        // Id do parâmetro de número máximo de tasks
        public  const short MaximoNumeroThreads = 1;
        
        // Id do parâmetro de intervalo de processamentos
        public const short IntervaloProcessamentos = 2;
        
        public short Id { get; set; }
        public string Chave { get; set; }
        public string Descricao { get; set; }
        public string ValorTexto { get; set; }
        public decimal? ValorNumerico { get; set; }
    }
}