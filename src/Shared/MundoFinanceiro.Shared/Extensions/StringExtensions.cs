using System.Text.RegularExpressions;

namespace MundoFinanceiro.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converte a string para o formato Snake Case, formato indicado para trabalhar com PostgreSQL.
        /// </summary>
        /// <param name="value">Valor que deve ser convertido</param>
        /// <returns></returns>
        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) 
                return value;

            var startUnderscores = Regex.Match(value, @"^_+");
            return startUnderscores + Regex.Replace(value, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        
        /// <summary>
        /// Remove os espaços em branco repetidos na string. 
        /// </summary>
        /// <param name="value">String que deve ser removido os espaços em branco</param>
        /// <returns></returns>
        public static string RemoveRepeatedWhitespaces(this string value)
        {
            const RegexOptions options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);     
            
            return regex.Replace(value, " ");
        }
    }

}