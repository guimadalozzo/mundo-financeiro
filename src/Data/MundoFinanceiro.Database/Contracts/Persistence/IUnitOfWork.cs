using System;
using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IFundamentoRepository Fundamentos { get; }
        IPapelRepository Papeis { get; }
        
        int Complete();
        Task<int> CompleteAsync();
    }
}