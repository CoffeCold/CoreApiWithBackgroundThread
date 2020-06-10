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
    public interface IJobLogsService
    {
        Task<IEnumerable<JobLog>> GetAsync();

    }

    public static class JobLogsServiceExtensions
    {
        public static void AddJobLogsService(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<IJobLogsService, JobLogsService>();
        }
    }

    public class JobLogsService : IJobLogsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _coreApiBaseAddress = string.Empty;

        public JobLogsService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _coreApiBaseAddress = configuration["CoreApi:ApiBaseAddress"];
        }

        public async Task<IEnumerable<JobLog>> GetAsync()
        {
            var response = await _httpClient.GetAsync($"{ _coreApiBaseAddress}/JobLogs");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<JobLog> JobLogs = JsonConvert.DeserializeObject<IEnumerable<JobLog>>(content);

                return JobLogs;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
    }
}
