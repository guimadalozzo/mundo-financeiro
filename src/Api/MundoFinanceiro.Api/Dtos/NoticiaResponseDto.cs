using System.Collections.Generic;

namespace MundoFinanceiro.Api.Dtos
{
    public class NoticiaResponseDto
    {
        public string Status { get; set; }
        public ICollection<NoticiaDto> Articles { get; set; }
    }
}