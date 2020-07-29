using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace JWTPolicyBasedAuthorization.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(AutoMapperMappingException amEx)
            {
                _logger.LogError($"AutoMapper Exception: {amEx}");
                await HandleExceptionAsync(context, amEx); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new Dtos.ErrorDto()
            {
                StatusCode = context.Response.StatusCode,
                Status = "Internal Server Error",
                Message = $"{ex.Message}"
            }.ToString());
        }

    }
}