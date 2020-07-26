using JWTPolicyBasedAuthorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTPolicyBasedAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Constants.Policy.AdminPolicy)]
    public class WeatherForecastController : ControllerBase
    {
        [Authorize(Policy = Constants.Permission.CanCreate)]
        [HttpGet]
        [Route("HasCreatePermission")]
        public string HasCreatePermission()
        {
            return "Has Create Permission";
        }

        [Authorize(Policy = Constants.Permission.CanDelete)]
        [HttpGet]
        [Route("HasDeletePermission")]
        public string HasDeletePermission()
        {
            return "Has Delete Permission";
        }

        [Authorize(Policy = Constants.Permission.CanApprove)]
        [HttpGet]
        [Route("HasApprovePermission")]
        public string HasApprovePermission()
        {
            return "Has Approve Permission";
        }

        [Authorize(Policy = Constants.Permission.CanView)]
        [HttpGet]
        [Route("HasViewPermission")]

        public string HasViewPermission()
        {
            return "Has View Permission";
        }
    }
}
