using System;
using System.Threading.Tasks;

namespace MundoFinanceiro.Api.Contracts.Services.Data
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", TimeSpan? timeout = null);
    }
}