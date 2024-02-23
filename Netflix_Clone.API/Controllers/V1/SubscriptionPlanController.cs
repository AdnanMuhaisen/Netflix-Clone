
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Commands;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class SubscriptionPlanController(
        ILogger<SubscriptionPlanController> logger,
        ISender sender)
        
        : BaseController<SubscriptionPlanController>(logger)
    {
        private readonly ISender sender = sender;

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<SubscriptionPlanDto>>>> GetSubscriptionPlans()
        {
            logger.LogTrace($"Try to get the subscription plans");

            var response = await sender.Send(new GetAllSubscriptionPlansQuery());

            if (response.IsSucceed && response.Result is not null)
            {
                return Ok(response);
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
            logger.LogTrace("Try to add a user subscription to the current user");

            var response = await sender.Send(new AddNewUserSubscriptionCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PlanId));

            if (response.IsSucceed && response.Result is not null)
            {
                return Created("", response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
