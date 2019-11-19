using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class FundamentoRepository : Repository<Fundamento>, IFundamentoRepository 
    {
        public FundamentoRepository(DataContext context) : base(context)
        {
            
        }

        /// <summary>
        /// Busca o fundamento da papel no dia, caso exista algum
        /// </summary>
        /// <param name="papelId">Id do papel</param>
        /// <returns>Fundamento do papel, caso exista algum</returns>
        public Task<Fundamento> BuscaFundamentoDiaAsync(short papelId)
        {
            var fundamentoDia = from f in Context.Fundamentos
                                where f.PapelId == papelId && f.Data.Date == DateTime.Now.Date
                                select f;

            return fundamentoDia.LastOrDefaultAsync();
        }
    }
}