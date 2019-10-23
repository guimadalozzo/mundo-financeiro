using System.Threading.Tasks;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Contracts.Persistence.Repositories
{
    public interface IFundamentoRepository : IRepository<Fundamento>
    {
        /// <summary>
        /// Busca o fundamento da papel no dia, caso exista algum
        /// </summary>
        /// <param name="papelId">Id do papel</param>
        /// <returns>Fundamento do papel, caso exista algum</returns>
        Task<Fundamento> BuscaFundamentoDiaAsync(short papelId);
    }
}