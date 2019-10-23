using System;

namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Fundamento
    {
        public int Id { get; set; }
        public short PapelId { get; set; }
        public float LPA { get; set; }
        public float VPA { get; set; }
        public float ROE { get; set; }
        public float ROIC { get; set; }
        public decimal ValorMercado { get; set; }
        public DateTime Data { get; set; }

        public virtual Papel Papel { get; set; }
    }
}