using AutoMapper;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Replicacao.Dtos;

namespace MundoFinanceiro.Replicacao.Profiles
{
    public class FundamentoProfile : Profile
    {
        public FundamentoProfile()
        {
            CreateMap<Fundamento, FundamentoDto>();
            CreateMap<FundamentoDto, Fundamento>();
        }
    }
}