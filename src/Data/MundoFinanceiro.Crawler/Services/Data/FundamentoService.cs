using System;
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
        private readonly IReplicacaoService _replicacaoService;

        public FundamentoService(IUnitOfWork unitOfWork, IFundamentoCrawler crawler, IReplicacaoService replicacaoService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _replicacaoService = replicacaoService ?? throw new ArgumentNullException(nameof(replicacaoService));
        }

        public async Task<Fundamento> ProcessarAsync(Papel papel)
        {
            // Processa o fundamento 
            var fundamento = await _crawler.ProcessarAsync(papel);

            // Salva o fundamento no banco de dados
            _unitOfWork.Fundamentos.Add(fundamento);
            await _unitOfWork.CompleteAsync();
            
            // Replica o fundamento
            _replicacaoService.ReplicarFundamento(fundamento);
            
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

            // Replica os fundamentos
            _replicacaoService.ReplicarFundamentos(fundamentos);
            
            // Retorna os fundamentos processados
            return fundamentos;
        }

        public void Dispose()
        {
            _crawler?.Dispose();
            _unitOfWork?.Dispose();
        }
    }
}