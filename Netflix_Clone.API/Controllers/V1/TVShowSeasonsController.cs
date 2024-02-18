using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Queries;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class TVShowSeasonsController(
        ILogger<TVShowSeasonsController> logger,
        ISender sender)
        
        : BaseController<TVShowSeasonsController>(logger)
    {
        private readonly ISender sender = sender;

        [HttpGet]
        [Route("GET/{TVShowContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowSeasonDto>>>> GetTVShowSeasons(int TVShowContentId)
        {
            var response = await sender.Send(new GetTVShowSeasonsQuery(TVShowContentId));

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
        [Route("POST/AddNewSeasonForTVShow")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<TVShowSeasonDto>>> AddNewSeasonForTVShow([FromBody] TVShowSeasonToInsertDto tVShowSeasonToInsertDto)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(tVShowSeasonToInsertDto.Adapt<AddNewTVShowSeasonCommand>());

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
        [Route("DELETE/TVShowSeason")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteTVShowSeason(
            [FromBody] DeleteTVShowSeasonRequestDto deleteTVShowSeasonRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await sender.Send(deleteTVShowSeasonRequestDto.Adapt<DeleteTVShowSeasonCommand>());

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
    }
}
