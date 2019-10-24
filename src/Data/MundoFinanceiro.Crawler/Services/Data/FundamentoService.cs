using System.Collections.Generic;
using System.Threading.Tasks;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Shared.Attributes;

namespace MundoFinanceiro.Crawler.Services.Data
{
    [MappedService]
    internal class FundamentoService : IFundamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFundamentoCrawler _crawler;

        public FundamentoService(IUnitOfWork unitOfWork, IFundamentoCrawler crawler)
        {
            _unitOfWork = unitOfWork;
            _crawler = crawler;
        }

        public async Task<Fundamento> ProcessarAsync(Papel papel)
        {
            // Processa o fundamento 
            var fundamento = await _crawler.ProcessarAsync(papel);

            // Salva o fundamento no banco de dados
            _unitOfWork.Fundamentos.Add(fundamento);
            await _unitOfWork.CompleteAsync();
            
            // Retorna o fundamento processado
            return fundamento;
        }

        public async Task<IEnumerable<Fundamento>> ProcessarAsync(IList<Papel> papeis)
        {
            // Processa os fundamentos
            var fundamentos = await _crawler.ProcessarAsync(papeis);
            
            // Salva os fundamentos
            _unitOfWork.Fundamentos.AddRange(fundamentos);
            await _unitOfWork.CompleteAsync();

            // Retorna os fundamentos processados
            return fundamentos;
        }
    }
}