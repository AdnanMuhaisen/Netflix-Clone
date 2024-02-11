using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Netflix_Clone.API.Controllers
{
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        protected readonly ILogger<T> logger;

        public BaseController(ILogger<T> logger)
        {
            this.logger = logger;
        }

    }
}
