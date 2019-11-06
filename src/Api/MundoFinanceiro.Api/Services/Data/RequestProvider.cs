using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MundoFinanceiro.Api.Contracts.Services.Data;
using MundoFinanceiro.Api.Exceptions;
using MundoFinanceiro.Shared.Attributes;
using MundoFinanceiro.Shared.Dtos;
using Newtonsoft.Json;

namespace MundoFinanceiro.Api.Services.Data
{
    [MappedService]
    public class RequestProvider : IRequestProvider
    {
        private const int TimeoutSeconds = 30;
        private const string MediaType = "application/json";
        private readonly Encoding _encoding = Encoding.UTF8;
        
        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", TimeSpan? timeout = null)
        {
            using var httpClient = CreateHttpClient(token, timeout);
            
            var response = await httpClient.GetAsync(uri);
            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }
        
        private static HttpClient CreateHttpClient(string token, TimeSpan? timeout)
        {
            var handler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var httpClient = new HttpClient(handler) { Timeout = timeout ?? TimeSpan.FromSeconds(TimeoutSeconds) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return httpClient;
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
                
                throw new AppException(responseDto.Message);
            }
        }
    }
}