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
    public interface IJobsService
    {
        Task<IEnumerable<Job>> GetAsync();

        Task<Job> GetAsync(string iban);

        Task DeleteAsync(string iban);

        Task<Job> AddAsync(Job Job);

        Task<Job> EditAsync(Job Job);
    }



    public static class JobsServiceExtensions
    {
        public static void AddJobsService(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<IJobsService, JobsService>();
        }
    }

    /// <summary></summary>
    /// <seealso cref="JobsClient.Services.IJobsService" />
    public class JobsService : IJobsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _coreApiBaseAddress = string.Empty;

        public JobsService( HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _coreApiBaseAddress = configuration["CoreApi:ApiBaseAddress"];
        }

        public async Task<IEnumerable<Job>> GetAsync()
        {
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Job");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<Job> Jobs = JsonConvert.DeserializeObject<IEnumerable<Job>>(content);

                return Jobs;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }



        public async Task<Job> GetAsync(string iban)
        {
            //api/Jobs/byIban?iban=12AS12432546789&withJobLogs=true
            //var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Jobs/{iban}");
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/Jobs/byIban?iban={iban}&withJobLogs=true");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Job Job = JsonConvert.DeserializeObject<Job>(content);

                return Job;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<Job> AddAsync(Job Job)
        {

            var jsonRequest = JsonConvert.SerializeObject(Job);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync($"{ _coreApiBaseAddress}/Jobs", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Job = JsonConvert.DeserializeObject<Job>(content);

                return Job;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task DeleteAsync(string iban)
        {

            var response = await _httpClient.DeleteAsync($"{ _coreApiBaseAddress}/Jobs/{iban}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<Job> EditAsync(Job Job)
        {

            var jsonRequest = JsonConvert.SerializeObject(Job);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PutAsync($"{ _coreApiBaseAddress}/Jobs/{Job.JobId}", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Job = JsonConvert.DeserializeObject<Job>(content);

                return Job;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }


    }
}



