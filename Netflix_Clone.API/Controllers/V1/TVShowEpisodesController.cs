
using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.Common.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class TVShowEpisodesController(
        ILogger<TVShowEpisodesController> logger,
        ISender sender)
        
        : BaseController<TVShowEpisodesController>(logger)
    {
        private readonly ISender sender = sender;

        //test this end point
        [HttpGet]
        [Route("GET/GetTVShowSeasonEpisodes")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowEpisodeDto>>>> GetAllTVShowSeasonEpisodes(
            [FromQuery] TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(tVShowSeasonEpisodesRequestDto.Adapt<GetTVShowSeasonEpisodesQuery>());

                if (response.IsSucceed && response.Result is not null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpPost]
        [Route("POST/AddTVShowEpisode")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]

        public async Task<ActionResult<ApiResponseDto<TVShowEpisodeDto>>> AddNewSeasonEpisode([FromBody] TVShowEpisodeToInsertDto tVShowEpisodeToInsert)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(tVShowEpisodeToInsert.Adapt<AddNewTVShowEpisodeCommand>());

                if (response.IsSucceed && response.Result is not null)
                {
                    return Created("", response);
                }
                else
                {
                    return BadRequest(response);
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
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteSeasonEpisode(
            [FromBody] TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(tVShowSeasonEpisodeToDeleteDto.Adapt<DeleteSeasonEpisodeCommand>());

                if (response.IsSucceed && response.Result.IsDeleted)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpGet]
        [Route("GET")]
        public async Task<ActionResult<ApiResponseDto<TVShowEpisodeDto>>> GetTVShowEpisode(
            [FromQuery] TVShowEpisodeRequestDto tVShowEpisodeRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(tVShowEpisodeRequestDto.Adapt<GetTVShowEpisodeQuery>());

                if (response.IsSucceed)
                {
                    if (response.Result is not null && User.Identity is not null && User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == USER_ROLE))
                    {
                        // add to user watch history
                        var command = new AddToUserWatchHistoryCommand(new AddToUserWatchHistoryRequestDto
                        {
                            ContentId = tVShowEpisodeRequestDto.TVShowId,
                            ApplicationUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                        });

                        await sender.Send(command);
                    }
                }
                else
                {
                    return BadRequest(response);
                }

                return response.Result is not null
                    ? Ok(response)
                    : NotFound();
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpPost]
        [Route("POST/DownloadTVShowEpisode")]
        public async Task<ActionResult<ApiResponseDto<string>>> DownloadTVShowEpisode(
            [FromBody] DownloadEpisodeRequestDto downloadEpisodeRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new DownloadTVShowEpisodeCommand(downloadEpisodeRequestDto, User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var response = await sender.Send(command);
                if (response.IsSucceed && response.Result is not null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }
    }
}
