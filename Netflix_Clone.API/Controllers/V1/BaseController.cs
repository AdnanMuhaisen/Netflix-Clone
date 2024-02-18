using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Netflix_Clone.API.Controllers.V1
{
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        protected readonly ILogger<T> logger;
        protected const string ADMIN_ROLE = "Admin";
        protected const string USER_ROLE = "User";
        protected const string BEARER_AUTHENTICATION_SCHEME = "Bearer";

        public BaseController(ILogger<T> logger)
        {
            this.logger = logger;
        }

    }
}
