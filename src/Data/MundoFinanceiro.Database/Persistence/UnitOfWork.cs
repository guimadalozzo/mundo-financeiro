using System;
using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;
using MundoFinanceiro.Database.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IFundamentoRepository _fundamentos;
        private IPapelRepository _papeis;
        private IParametroRepository _parametros;

        public UnitOfWork(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            
            _context = new DataContext(connectionString);
        }
        
        public IFundamentoRepository Fundamentos => _fundamentos ?? (_fundamentos = new FundamentoRepository(_context));
        public IPapelRepository Papeis => _papeis ?? (_papeis = new PapelRepository(_context));
        public IParametroRepository Parametros => _parametros ?? (_parametros = new ParametroRepository(_context));

        public int Complete() => _context.SaveChanges();

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}