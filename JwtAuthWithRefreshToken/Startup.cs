using System;
using System.IO;
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
using JwtAuthWithRefreshToken.Infrastructure;
using NLog;
using AutoMapper;

namespace JwtAuthWithRefreshToken
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // NLog load configuration  
            var dirPath = Directory.GetCurrentDirectory();
            LogManager.LoadConfiguration($@"{dirPath}\nlog.config");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Logger Service
            services.ConfigureLoggerService();

            // Configure SQL database
            services.ConfigureSqlDbConnection(Configuration.SqlConnectionString());

            // Configure Services
            services.ConfigureServices();

            // Configure Controllers
            services.AddControllers();

            // Add Automapper
            services.AddAutoMapper(typeof(Startup));

            // Add cors
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
