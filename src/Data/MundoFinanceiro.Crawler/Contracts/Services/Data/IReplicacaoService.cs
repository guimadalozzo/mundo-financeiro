using System.Collections.Generic;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Crawler.Contracts.Services.Data
{
    public interface IReplicacaoService
    {
        void ReplicarFundamento(Fundamento fundamento);
        void ReplicarFundamentos(ICollection<Fundamento> fundamentos);
    }
}