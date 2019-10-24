using AutoMapper;
using MundoFinanceiro.Crawler.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Crawler.Profiles
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