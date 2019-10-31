using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MundoFinanceiro.Crawler.Contracts.Services.Data;
using MundoFinanceiro.Crawler.Dtos;
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
        private readonly IMapper _mapper;
        
        public FundamentoService(IUnitOfWork unitOfWork, IFundamentoCrawler crawler, IReplicacaoService replicacaoService, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
            _replicacaoService = replicacaoService ?? throw new ArgumentNullException(nameof(replicacaoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Fundamento> ProcessarAsync(Papel papel)
        {
            // Processa o fundamento 
            var fundamento = await _crawler.ProcessarAsync(papel);

            // Salva o fundamento no banco de dados
            _unitOfWork.Fundamentos.Add(fundamento);
            await _unitOfWork.CompleteAsync();
            
            // Replica o fundamento nos nodos de replicação
            var nodosReplicacao = await BuscarReplicacoesAtivas();
            _replicacaoService.Replicar(nodosReplicacao, "v1/Fundamentos", _mapper.Map<FundamentoDto>(fundamento));
            
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

            // Replica o fundamento nos nodos de replicação
            var nodosReplicacao = await BuscarReplicacoesAtivas();
            _replicacaoService.Replicar(nodosReplicacao, "v1/Fundamentos", _mapper.Map<ICollection<FundamentoDto>>(fundamentos));

            // Retorna os fundamentos processados
            return fundamentos;
        }

        private async Task<ICollection<string>> BuscarReplicacoesAtivas() => await  _unitOfWork.Replicacoes.Find(x => x.Ativo).Select(x => x.Url).ToListAsync();

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            _crawler?.Dispose();
        }
    }
}