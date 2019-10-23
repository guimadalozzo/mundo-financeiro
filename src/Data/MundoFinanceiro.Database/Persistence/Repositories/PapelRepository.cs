using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class PapelRepository : Repository<Papel>, IPapelRepository
    {
        public PapelRepository(DataContext context) : base(context)
        {
            
        }
    }
}