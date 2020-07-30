using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data;
using JWTPolicyBasedAuthorization.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using System;
using Microsoft.OpenApi.Models;
using System.IO;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using JWTPolicyBasedAuthorization.Dtos;

namespace JWTPolicyBasedAuthorization.Extensions
{
    /// <summary>
    /// Extension class to configure extended services 
    /// </summary>
    public static class ServicesExtensions
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(optionsAction =>
                optionsAction.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnectionString")
                )
            );
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static void ConfigureSeedData(this IServiceCollection services)
        {
            services.AddTransient<Data.SeedData.Seed>();
        }

        public static void ConfigureJwtBearerToken(this IServiceCollection services, IConfiguration configuration)
        {
            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

        }

        public static void ConfigureAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Policy.AdminPolicy,
                        policy => policy.RequireRole(
                            Constants.Role.SuperAdmin,
                            Constants.Role.SystemAdmin)
                        );

                options.AddPolicy(Constants.Policy.ManagerPolicy,
                        policy => policy.RequireRole(
                            Constants.Role.RegionalManager,
                            Constants.Role.DepartmentManager)
                        );

                options.AddPolicy(Constants.Policy.UserPolicy,
                        policy => policy.RequireRole(
                            Constants.Role.Reviewer,
                            Constants.Role.Owner,
                            Constants.Role.TeamMember)
                        );

                options.AddPolicy(Constants.Permission.CanCreate, policy => policy.RequireClaim(Constants.Permission.CanCreate));
                options.AddPolicy(Constants.Permission.CanDelete, policy => policy.RequireClaim(Constants.Permission.CanDelete));
                options.AddPolicy(Constants.Permission.CanApprove, policy => policy.RequireClaim(Constants.Permission.CanApprove));
                options.AddPolicy(Constants.Permission.CanView, policy => policy.RequireClaim(Constants.Permission.CanView));
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Set the comments summary of api methods
                // Note: Add below line in .csproj inside PropertyGroup
                // <GenerateDocumentationFile>true</GenerateDocumentationFile>    
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                //Generate the Default UI of Swagger Documentation 
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Title:JWT Token Authentication API",
                    Version = "Version: v1",
                    Description = "Description: JWT Authentication & Authorization with ASP.NET Core Identity and Swagger documentation with Authorization.",
                    TermsOfService = new Uri("https://app.swaggerhub.com/help/index"),
                    Contact = new OpenApiContact
                    {
                        Name = "Sagar Khairnar",
                        Email = "sagarkhairnar@test.com",
                        Url = new Uri("https://twitter.com/sagarkhairnar"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "JWT Token Authentication API",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Enable authorization using Swagger (JWT)  
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationBadRequestFilter>();
            services.AddScoped<AsyncValidationBadRequestFilter>();
        }


        public static void ConfigureHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnectionString");
            services.AddHealthChecks()
            .AddCheck("Db Health",()=> HealthCheckProvider.CheckDb(connString));
        }
    }
}