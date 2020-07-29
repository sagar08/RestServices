using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTPolicyBasedAuthorization.Infrastructure
{
    public abstract class ValidateEntityExistsHandler<T> where T : class, IEntity
    {
        private ApplicationDbContext _context;

        public ValidateEntityExistsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsValidEntity(int id)
        {
            var entity = _context.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            return entity != null;
        }
    }
    public class ValidateEntityExistsFilter<T>
            : ValidateEntityExistsHandler<T>, IActionFilter
             where T : class, IEntity
    {
        public ValidateEntityExistsFilter(ApplicationDbContext context) : base(context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = ((IEntity)context.ActionArguments.SingleOrDefault().Value).Id;
            if (!IsValidEntity(id)) return;
        }
    }

    public class AsyncValidateEntityExistsFilter<T>
            : ValidateEntityExistsHandler<T>, IAsyncActionFilter
             where T : class, IEntity
    {
        public AsyncValidateEntityExistsFilter(ApplicationDbContext context) : base(context)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = ((IEntity)context.ActionArguments.SingleOrDefault().Value).Id;
            if (!IsValidEntity(id)) return;
            var result = await next();
        }
    }
}