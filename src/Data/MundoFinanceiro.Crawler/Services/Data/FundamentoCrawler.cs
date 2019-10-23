using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Crawler.Services.Data
{
    [MappedService]
    internal class FundamentoCrawler : IFundamentoCrawler
    {
        public Task<Fundamento> ProcessarAsync(Papel papel)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Fundamento>> ProcessarAsync(IEnumerable<Papel> papeis)
        {
            throw new System.NotImplementedException();
        }
    }
}