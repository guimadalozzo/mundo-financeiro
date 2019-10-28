using System.Collections.Generic;

namespace MundoFinanceiro.Database.Contracts.Persistence.Domains
{
    public class Papel
    {
        public Papel()
        {
            Fundamentos = new HashSet<Fundamento>();
        }
        
        public short Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Fundamento> Fundamentos { get; set; }
    }
}