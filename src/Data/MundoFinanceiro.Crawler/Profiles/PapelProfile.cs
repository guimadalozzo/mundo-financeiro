using AutoMapper;
using MundoFinanceiro.Crawler.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Crawler.Profiles
{
    public class PapelProfile : Profile
    {
        public PapelProfile()
        {
            CreateMap<Papel, PapelDto>();
            CreateMap<PapelDto, Papel>();
        }
    }
}