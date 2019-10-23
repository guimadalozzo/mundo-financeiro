using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal class PapelRepository : Repository<Papel>, IPapelRepository
    {
        public PapelRepository(DataContext context) : base(context)
        {
            
        }

        /// <summary>
        /// Busca os papeis não processados no dia corrente
        /// </summary>
        /// <returns>IEnumerable contendo os papéis pendentes de processamento</returns>
        public async Task<IList<Papel>> BuscaPapeisPendentesAsync()
        {
            var papeisProcessadosDia =
                from f in Context.Fundamentos
                where f.Data.Date == DateTime.Now.Date
                select f.PapelId;

            var papeisNaoProcessados =
                from p in Context.Papeis
                where !papeisProcessadosDia.Contains(p.Id)
                select p;

            return await papeisNaoProcessados.ToListAsync();
        }
    }
}