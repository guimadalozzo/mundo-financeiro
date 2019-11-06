using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Api.Dtos;

namespace MundoFinanceiro.Api.Contracts.Services.Data
{
    public interface IHistoricoService
    {
        /// <summary>
        /// Busca o histórico do papel informado
        /// </summary>
        /// <param name="papel">Papel que se deseja obter o histórico</param>
        /// <returns></returns>
        Task<IEnumerable<HistoricoDto>> BuscarHistoricoPapelAsync(string papel);
    }
}