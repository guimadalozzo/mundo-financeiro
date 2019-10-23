using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Contracts.Persistence.Repositories
{
    public interface IPapelRepository : IRepository<Papel>
    {
        Task<IList<Papel>> BuscaPapeisPendentesAsync();
    }
}