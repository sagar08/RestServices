using System.Collections.Generic;
using System.IO;
using AutoMapper;
using JWTPolicyBasedAuthorization.Extensions;
using JWTPolicyBasedAuthorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace JWTPolicyBasedAuthorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var dirPath = Directory.GetCurrentDirectory();
            LogManager.LoadConfiguration($@"{Directory.GetCurrentDirectory()}\nlog.config");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add Database connection
            services.ConfigureDatabaseConnection(Configuration);

            // Add Logger Service
            services.ConfigureLoggerService();

            // Add Repository
            services.ConfigureRepositories();

            // Add Seed Data
            services.ConfigureSeedData();

            services.AddControllers();

            // Add Action filters
            services.ConfigureActionFilters();

            // Add JWT Bearer authorization
            services.ConfigureJwtBearerToken(Configuration);

            // Add Swagger Documentation
            services.ConfigureSwagger();

            // Policy based authorization
            services.ConfigureAuthorizationPolicy();

            // Add Auto mapper
            services.AddAutoMapper(typeof(Startup));

            // Add Cord
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // configure Exception handler
            //app.ConfigureExceptionHandler(logger);
            app.ConfigureExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
        }
    }
}
