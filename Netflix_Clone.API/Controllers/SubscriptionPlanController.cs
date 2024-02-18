
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Commands;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
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
        public async Task<ActionResult<ApiResponseDto<IEnumerable<SubscriptionPlanDto>>>> GetSubscriptionPlans()
        {
            var response = await mediator.Send(new GetAllSubscriptionPlansQuery());

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? Ok(response)
                    : NotFound();
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPost]
        [Route("POST/NewUserSubscription/{PlanId:int}")]
        public async Task<ActionResult<ApiResponseDto<UserSubscriptionPlanDto>>> AddNewUserSubscription(int PlanId)
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var response = await mediator.Send(new AddNewUserSubscriptionCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PlanId));

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? Created("", response)
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
