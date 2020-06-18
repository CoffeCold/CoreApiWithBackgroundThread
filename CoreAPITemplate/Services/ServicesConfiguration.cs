using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoreAPI.Services
{
    public static class ServiceCollectionExtensions
    {

 
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<JobsToRunSingleton, JobsToRunSingleton>();
            services.AddHostedService<JobBackgroundService_1>();
            services.AddHostedService<JobBackgroundService_2>();
            services.AddHostedService<JobBackgroundService_3>();
            services.AddScoped<IJobManagementService, JobManagementService>();
            services.AddTransient<IBatchService, BatchService>();
            services.AddTransient<IBulkService, BulkService>();
        }

        public static void ConfigureCors(this IServiceCollection services,string  APIOrigins)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: APIOrigins,
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowedToAllowWildcardSubdomains()
      .WithOrigins("*")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .Build();
                                  });
            });

        }
    }
}
