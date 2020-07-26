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

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
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
    }
}