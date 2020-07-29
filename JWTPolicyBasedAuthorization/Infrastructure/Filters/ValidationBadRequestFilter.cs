using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTPolicyBasedAuthorization.Infrastructure
{

    public abstract class BadRequestHandler
    {
        public bool IsBadRequest(ActionExecutingContext context)
        {
            var result = false;
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Object is null"
                });
                result = true;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                result = true;
            }

            return result;
        }
    }

    public class ValidationBadRequestFilter : BadRequestHandler, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // execute any code after the action executes            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // execute any code before the action executes            
            var badRequest = IsBadRequest(context);
            if (badRequest) return;
        }
    }

    public class AsyncValidationBadRequestFilter : BadRequestHandler, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var badRequest = IsBadRequest(context);
            if (badRequest) return;

            var result = await next();

            // execute any code after the action executes

        }
    }
}