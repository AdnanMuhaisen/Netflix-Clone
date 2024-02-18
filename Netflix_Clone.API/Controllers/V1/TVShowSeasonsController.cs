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
    public class TVShowSeasonsController : BaseController<TVShowSeasonsController>
    {
        private readonly IMediator mediator;

        public TVShowSeasonsController(ILogger<TVShowSeasonsController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("GET/{TVShowContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowSeasonDto>>>> GetTVShowSeasons(int TVShowContentId)
        {
            var response = await mediator.Send(new GetTVShowSeasonsQuery(TVShowContentId));

            if (response.IsSucceed)
            {
                return response.Result is not null
                    ? Ok(response)
                    : NotFound();
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
                var response = await mediator.Send(tVShowSeasonToInsertDto.Adapt<AddNewTVShowSeasonCommand>());

                if (response.IsSucceed)
                {
                    return response.Result is not null
                        ? Created("", response)
                        : BadRequest(response);
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
                var response = await mediator.Send(deleteTVShowSeasonRequestDto.Adapt<DeleteTVShowSeasonCommand>());

                if (response.IsSucceed)
                {
                    return response.Result.IsDeleted
                        ? NoContent()
                        : BadRequest(response);
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
