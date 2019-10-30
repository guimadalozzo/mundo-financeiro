using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class ReplicacaoRepository : Repository<Replicacao>, IReplicacaoRepository
    {
        public ReplicacaoRepository(DataContext context) : base(context)
        {
            
        }
    }
}