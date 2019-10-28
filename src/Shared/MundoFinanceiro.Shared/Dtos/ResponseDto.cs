namespace MundoFinanceiro.Shared.Dtos
{
    /// <summary>
    /// Response common utilizada nas APIs da solução
    /// </summary>
    public class ResponseDto
    {
        public ResponseDto(string message, string internalMessage = null)
        {
            Message = message;
            InternalMessage = internalMessage;
        }

        public string Message { get; }
        public string InternalMessage { get; }
    }
}