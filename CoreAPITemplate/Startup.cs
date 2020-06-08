using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;
using CoreAPI.Helpers;
using CoreAPI.Services;


namespace CoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string AllThinkableOrigins = "_myAllThinkableOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO Narrow CORS Target.
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllThinkableOrigins,
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowedToAllowWildcardSubdomains()
      .WithOrigins("*")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .Build();
                                  });
            });

            // Connection string used for DI context
            var connectionStringsSection = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(connectionStringsSection);
            var connectionstrings = connectionStringsSection.Get<ConnectionStrings>();


            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionsService, TransactionsService>();

            services.AddControllers();
            services.AddDbContext<TransactionDBContext>(options =>
     options.UseSqlServer(connectionstrings.TransactionDBConstr));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configure called");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(AllThinkableOrigins);

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation("Configure ended");

        }
    }
}
