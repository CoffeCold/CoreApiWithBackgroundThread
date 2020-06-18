using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CoreAPI.ConfigurationSettings;
using CoreAPI.Models;

namespace CoreAPI.Context
{

    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration Configuration)
        {
            // Connection string used for DI context
            var connectionStringsSection = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(connectionStringsSection);
            var connectionstrings = connectionStringsSection.Get<ConnectionStrings>();

            services.AddDbContext<JobManagementDBContext>(options =>
              options.UseSqlServer(connectionstrings.JobManagementDBConstr));

        }
    }

}
