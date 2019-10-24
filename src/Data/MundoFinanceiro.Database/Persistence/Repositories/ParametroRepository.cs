using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class ParametroRepository : Repository<Parametro>, IParametroRepository
    {
        public ParametroRepository(DataContext context) : base(context)
        {
            
        }

        public async Task<int> BuscarValorInteiroAsync(short id, int defaultValue = 0)
        {
            var parametro = await GetAsync(id);
            return parametro?.ValorNumerico == null
                ? defaultValue
                : (int) parametro.ValorNumerico.Value;
        }
    }
}