using JwtAuthWithRefreshToken.Data;
using JwtAuthWithRefreshToken.Models;
using JwtAuthWithRefreshToken.Services;
using JwtAuthWithRefreshToken.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JwtAuthWithRefreshToken.Infrastructure
{
    public static class ServiceExtensions
    {
        public static string SqlConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("SqlConnectionString");
        }

        public static void ConfigureSqlDbConnection(this IServiceCollection services, string connString)
        {
            services.AddDbContext<SqlDbContext>(optionsAction =>
            optionsAction.UseSqlServer(connString));
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}