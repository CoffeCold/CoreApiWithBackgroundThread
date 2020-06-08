using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using CoreAPI.Models;

namespace CoreMVCClient.Services
{
    public interface ITransactionsService
    {
        Task<IEnumerable<Transaction>> GetAsync();

    }

    public static class TransactionsServiceExtensions
    {
        public static void AddTransactionsService(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<ITransactionsService, TransactionsService>();
        }
    }

    public class TransactionsService : ITransactionsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _coreApiBaseAddress = string.Empty;

        public TransactionsService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _coreApiBaseAddress = configuration["CoreApi:ApiBaseAddress"];
        }

        public async Task<IEnumerable<Transaction>> GetAsync()
        {
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Transactions");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<Transaction> Transactions = JsonConvert.DeserializeObject<IEnumerable<Transaction>>(content);

                return Transactions;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
    }
}
