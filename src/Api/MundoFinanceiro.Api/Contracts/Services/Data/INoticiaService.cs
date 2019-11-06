using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Api.Dtos;

namespace MundoFinanceiro.Api.Contracts.Services.Data
{
    public interface INoticiaService
    {
        /// <summary>
        /// Busca as últimas notícias de um determinado assunto.
        /// </summary>
        /// <param name="assunto">Assunto das notícias</param>
        /// <returns></returns>
        Task<IEnumerable<NoticiaDto>> BuscarNoticiasAsync(string assunto);
    }
}