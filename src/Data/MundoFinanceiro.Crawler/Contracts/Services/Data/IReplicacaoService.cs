using System.Collections.Generic;

namespace MundoFinanceiro.Crawler.Contracts.Services.Data
{
    public interface IReplicacaoService
    {
        /// <summary>
        /// Replica o objeto informado no endpoint informado nas urls de replicação informadas
        /// </summary>
        /// <param name="urls">Urls de replicação</param>
        /// <param name="endpoint">Endpoint de replicação</param>
        /// <param name="objeto">Objeto que deve ser replicado</param>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        void Replicar<T>(ICollection<string> urls, string endpoint, T objeto);
        
        /// <summary>
        /// Replica os objeto informados no endpoint informado nas urls de replicação informadas
        /// </summary>
        /// <param name="urls">Urls de replicação</param>
        /// <param name="endpoint">Endpoint de replicação</param>
        /// <param name="objetos">Objetos que devem ser replicados</param>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        void Replicar<T>(ICollection<string> urls, string endpoint, ICollection<T> objetos);
    }
}