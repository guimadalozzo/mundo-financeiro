namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Replicacao
    {
        public byte Id { get; set; }
        public string Url { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}