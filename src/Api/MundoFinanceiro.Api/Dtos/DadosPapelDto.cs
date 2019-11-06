using System.Collections.Generic;

namespace MundoFinanceiro.Api.Dtos
{
    public class DadosPapelDto
    {
        public DadosPapelDto(PapelDto papel, FundamentoDto fundamentos, IEnumerable<HistoricoDto> historico, IEnumerable<NoticiaDto> noticias)
        {
            Papel = papel;
            Fundamentos = fundamentos;
            Historico = historico;
            Noticias = noticias;
        }

        public PapelDto Papel { get; }
        public FundamentoDto Fundamentos { get; }
        public IEnumerable<HistoricoDto> Historico { get; }
        public IEnumerable<NoticiaDto> Noticias { get; }
    }
}