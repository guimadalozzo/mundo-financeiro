using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Contracts.Persistence.Repositories
{
    public interface IParametroRepository : IRepository<Parametro>
    {
        /// <summary>
        /// Busca o valor inteiro do parâmetro passado pela chave. Retorna o valor padrão caso não encontrado.
        /// </summary>
        /// <param name="id">Id do parâmetro</param>
        /// <param name="defaultValue">Valor padrão caso não encontrado</param>
        /// <returns></returns>
        Task<int> BuscarValorInteiroAsync(short id, int defaultValue = 0);
    }
}