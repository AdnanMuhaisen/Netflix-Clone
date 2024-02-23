
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class UserWatchListController(
        ILogger<UserWatchListController> logger,
        ISender sender)
        
        : BaseController<UserWatchListController>(logger)
    {
        private readonly ISender sender = sender;

        [HttpGet]
        [Route("GET")]
        public async Task<ActionResult<ApiResponseDto<UserWatchListDto>>> GetUserWatchList()
        {
            //This endpoint will create a watchlist for the user if the user has not created the watchlist 
            logger.LogTrace($"Try to get the user watch list");

            var response = await sender.Send(new GetUserWatchListQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value));

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
        [Route("POST/AddToUserWatchlist/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddToUserWatchList([FromRoute] int ContentId)
        {
            logger.LogTrace($"Try to add to the user watch list for the current user");

            var response = await sender.Send(new AddToUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, ContentId));

            if (response.IsSucceed && response.Result)
            {
                return Ok(response);
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
            logger.LogTrace($"Try to delete the content with id : {ContentId} from the current user watch list");

            var response = await sender.Send(new DeleteFromUserWatchListCommand(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                ContentId));

            if (response.IsSucceed && response.Result is not null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}
