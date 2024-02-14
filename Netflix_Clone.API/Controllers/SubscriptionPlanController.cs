
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME)]
    public class SubscriptionPlanController : BaseController<SubscriptionPlanController>
    {
        private readonly IMediator mediator;

        public SubscriptionPlanController(ILogger<SubscriptionPlanController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponseDto>> GetSubscriptionPlans()
        {
            var query = new GetAllSubscriptionPlansQuery();
            var response = await mediator.Send(query);

            return (response.Result is null) ? NotFound() : Ok(response);
        }


        [HttpPost]
        [Route("POST/NewUserSubscription/{PlanId:int}")]
        public async Task<ActionResult<ApiResponseDto>> AddNewUserSubscription(int PlanId)
        {
            if(User.Identity is null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var command = new AddNewUserSubscriptionCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, PlanId);
            var result = await mediator.Send(command);
            return (result.Result is null) ? BadRequest(result) : Created("", result);
        }

    }
}
