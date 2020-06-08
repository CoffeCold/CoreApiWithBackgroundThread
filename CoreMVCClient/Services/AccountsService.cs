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
    public interface IAccountsService
    {
        Task<IEnumerable<Account>> GetAsync();

        Task<Account> GetAsync(string iban);

        Task DeleteAsync(string iban);

        Task<Account> AddAsync(Account Account);

        Task<Account> EditAsync(Account Account);
    }



    public static class AccountsServiceExtensions
    {
        public static void AddAccountsService(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<IAccountsService, AccountsService>();
        }
    }

    /// <summary></summary>
    /// <seealso cref="AccountsClient.Services.IAccountsService" />
    public class AccountsService : IAccountsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _coreApiBaseAddress = string.Empty;

        public AccountsService( HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _coreApiBaseAddress = configuration["CoreApi:ApiBaseAddress"];
        }

        public async Task<IEnumerable<Account>> GetAsync()
        {
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Accounts");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<Account> Accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(content);

                return Accounts;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }



        public async Task<Account> GetAsync(string iban)
        {
            //api/Accounts/byIban?iban=12AS12432546789&withTransactions=true
            //var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Accounts/{iban}");
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Accounts/byIban?iban={iban}&withTransactions=true");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Account Account = JsonConvert.DeserializeObject<Account>(content);

                return Account;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<Account> AddAsync(Account account)
        {

            var jsonRequest = JsonConvert.SerializeObject(account);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync($"{ _coreApiBaseAddress}/Accounts", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                account = JsonConvert.DeserializeObject<Account>(content);

                return account;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task DeleteAsync(string iban)
        {

            var response = await _httpClient.DeleteAsync($"{ _coreApiBaseAddress}/Accounts/{iban}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<Account> EditAsync(Account Account)
        {

            var jsonRequest = JsonConvert.SerializeObject(Account);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PutAsync($"{ _coreApiBaseAddress}/Accounts/{Account.Iban}", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Account = JsonConvert.DeserializeObject<Account>(content);

                return Account;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }


    }
}



