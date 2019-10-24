using System;

namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Fundamento
    {
        public int Id { get; set; }
        public short PapelId { get; set; }
        public double LPA { get; set; }
        public double VPA { get; set; }
        public double ROE { get; set; }
        public double ROIC { get; set; }
        public decimal ValorMercado { get; set; }
        public DateTime Data { get; set; }

        public virtual Papel Papel { get; set; }
    }
}