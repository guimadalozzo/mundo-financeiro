using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class FundamentoRepository : Repository<Fundamento>, IFundamentoRepository 
    {
        public FundamentoRepository(DataContext context) : base(context)
        {
            
        }
    }
}