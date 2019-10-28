using AutoMapper;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Web.Dtos;

namespace MundoFinanceiro.Web.Profiles
{
    public class PapelProfile : Profile
    {
        public PapelProfile()
        {
            CreateMap<Papel, PapelDisponivelDto>();
            CreateMap<PapelDisponivelDto, Papel>();
        }
    }
}