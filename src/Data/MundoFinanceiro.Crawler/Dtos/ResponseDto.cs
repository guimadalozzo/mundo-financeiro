namespace MundoFinanceiro.Crawler.Dtos
{
    public class ResponseDto
    {
        public ResponseDto(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}