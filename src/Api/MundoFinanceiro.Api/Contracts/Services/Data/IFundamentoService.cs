using System;
using System.Threading.Tasks;
using MundoFinanceiro.Api.Dtos;

namespace MundoFinanceiro.Api.Contracts.Services.Data
{
    public interface IFundamentoService : IDisposable
    {
        Task<FundamentoDto> BuscarFundamentoAsync(short id);
    }
}