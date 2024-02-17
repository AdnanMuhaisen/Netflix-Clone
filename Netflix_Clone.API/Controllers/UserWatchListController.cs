
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
        public async Task<ActionResult<ApiResponseDto>> GetUserWatchList()
        {
            //This endpoint will create a watchlist for the user if the user has not created the watchlist 

            var query = new GetUserWatchListQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var response = await mediator.Send(query);
            return (response.Result is null) ? BadRequest(response) : Ok(response.Result);
        }

        [HttpPost]
        [Route("POST/AddToUserWatchlist/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto>> AddToUserWatchList([FromRoute] int ContentId)
        {
            var command = new AddToUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                ContentId);
            var response = await mediator.Send(command);
            return (response.Result is null) ? BadRequest(response) : Ok(response);
        }

        [HttpDelete]
        [Route("DELETE/DeleteFromUserWatchlist/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto>> DeleteFromUserWatchlist([FromRoute] int ContentId)
        {
            var command = new DeleteFromUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                ContentId);
            var response = await mediator.Send(command);
            return (response.Result is null) ? BadRequest(response) : NoContent();
        }
    }
}
