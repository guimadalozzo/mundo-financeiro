using AutoMapper;
using MundoFinanceiro.Api.Dtos;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Api.Profiles
{
    public class PapelProfile : Profile
    {
        public PapelProfile()
        {
            CreateMap<PapelDto, Papel>();
            CreateMap<Papel, PapelDto>();
        }
    }
}