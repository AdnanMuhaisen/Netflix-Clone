
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class TVShowEpisodesController : BaseController<TVShowEpisodesController>
    {
        private readonly IMediator mediator;

        public TVShowEpisodesController(ILogger<TVShowEpisodesController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        //test this end point
        [HttpGet]
        [Route("GET/GetTVShowSeasonEpisodes")]
        public async Task<ActionResult<ApiResponseDto>> GetAllTVShowSeasonEpisodes(
            [FromQuery] TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto)
        {
            if (ModelState.IsValid)
            {
                var query = new GetTVShowSeasonEpisodesQuery(tVShowSeasonEpisodesRequestDto);
                var response = await mediator.Send(query);
                return (response is null) ? BadRequest() : Ok(response);
            }
            else 
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpPost]
        [Route("POST/AddTVShowEpisode")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]

        public async Task<ActionResult<ApiResponseDto>> AddNewSeasonEpisode([FromBody] TVShowEpisodeToInsertDto tVShowEpisodeToInsert)
        {
            if (ModelState.IsValid)
            {
                var command = new AddNewTVShowEpisodeCommand(tVShowEpisodeToInsert);
                try
                {
                    var result = await mediator.Send(command);
                    return Created("", result);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }


        [HttpDelete]
        [Route("DELETE/DeleteEpisode")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> DeleteSeasonEpisode([FromBody] TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto)
        {
            if (ModelState.IsValid)
            {
                var command = new DeleteSeasonEpisodeCommand(tVShowSeasonEpisodeToDeleteDto);
                var response = await mediator.Send(command);
                return (((DeletionResultDto)response.Result).IsDeleted) ? NoContent() : BadRequest(response);
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpGet]
        [Route("GET")]
        public async Task<ActionResult<ApiResponseDto>> GetTVShowEpisode(
            [FromQuery] TVShowEpisodeRequestDto tVShowEpisodeRequestDto)
        {
            if (ModelState.IsValid)
            {
                var query = new GetTVShowEpisodeQuery(tVShowEpisodeRequestDto);
                var getEpisodeResponse = await mediator.Send(query);

                if (getEpisodeResponse.Result is not null
                    && User.Identity is not null
                    && User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == USER_ROLE))
                {
                    // add to user watch history
                    var command = new AddToUserWatchHistoryCommand(new AddToUserWatchHistoryRequestDto
                    {
                        ContentId = tVShowEpisodeRequestDto.TVShowId,
                        ApplicationUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                    });

                    await mediator.Send(command);
                }

                return (getEpisodeResponse.Result is null)
                    ? NotFound()
                    : Ok(getEpisodeResponse);
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpPost]
        [Route("POST/DownloadTVShowEpisode")]
        public async Task<ActionResult<ApiResponseDto>> DownloadTVShowEpisode(
            [FromBody] DownloadEpisodeRequestDto downloadEpisodeRequestDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new DownloadTVShowEpisodeCommand(downloadEpisodeRequestDto,
                        User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                    var response = await mediator.Send(command);
                    return (response.Result is null)
                        ? BadRequest(response)
                        : Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }         
    }
}
