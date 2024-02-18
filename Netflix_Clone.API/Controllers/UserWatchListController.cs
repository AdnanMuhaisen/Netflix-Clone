
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class UserWatchListController : BaseController<UserWatchListController>
    {
        private readonly IMediator mediator;

        public UserWatchListController(ILogger<UserWatchListController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("GET")]
        public async Task<ActionResult<ApiResponseDto<UserWatchListDto>>> GetUserWatchList()
        {
            //This endpoint will create a watchlist for the user if the user has not created the watchlist 

            var response = await mediator.Send(new GetUserWatchListQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value));

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
        [Route("POST/AddToUserWatchlist/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddToUserWatchList([FromRoute] int ContentId)
        {            
            var response = await mediator.Send(new AddToUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, ContentId));

            if (response.IsSucceed)
            {
                return (response.Result)
                    ? Ok(response)
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete]
        [Route("DELETE/DeleteFromUserWatchlist/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteFromUserWatchlist([FromRoute] int ContentId)
        {
            var response = await mediator.Send(new DeleteFromUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                ContentId));

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? NoContent()
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        
        }
    }
}
