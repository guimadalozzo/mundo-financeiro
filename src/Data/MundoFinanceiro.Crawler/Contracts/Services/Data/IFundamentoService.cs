using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Crawler.Contracts.Services.Data
{
    public interface IFundamentoService : IDisposable
    {
        Task<Fundamento> ProcessarAsync(Papel papel);
        Task<IEnumerable<Fundamento>> ProcessarAsync(IList<Papel> papeis);
    }
}