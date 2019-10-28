using System;

namespace MundoFinanceiro.Api.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
            
        }

        public AppException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}