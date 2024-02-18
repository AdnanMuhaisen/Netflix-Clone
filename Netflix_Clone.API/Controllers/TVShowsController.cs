using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class TVShowsController : BaseController<TVShowsController>
    {
        private readonly IMediator mediator;

        public TVShowsController(ILogger<TVShowsController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("GET")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetAllTVShows()
        {
            var response = await mediator.Send(new GetAllTVShowsQuery());

            if(response.IsSucceed)
            {
                return (response.Result is not null)
                    ? Ok(response)
                    : BadRequest();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("POST/AddNewTVShow")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<TVShowDto>>> AddNewTVShow([FromBody] TVShowToInsertDto tVShowToInsertDto)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(tVShowToInsertDto.Adapt<AddNewTVShowCommand>());

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
            else
            {
                return BadRequest(new ApiResponseDto<TVShowDto> 
                { 
                    Result = null!,
                    Message = "Invalid Model State",
                    IsSucceed = true
                });
            }
        }

        [HttpDelete]
        [Route("DELETE/{TVShowId:int}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteTVShow([FromRoute] int TVShowId)
        {
            // there`s a cascade delete between the tbl_TVShows table and the tbl_TVShowSeasons table 
            // but to avoid the cycles or multiple cascade paths problem : i have created a trigger 
            // to delete the season episodes when the season is deleted.

            var response = await mediator.Send(new DeleteTVShowCommand(TVShowId));

            if (response.IsSucceed)
            {
                return (response.Result.IsDeleted)
                    ? NoContent()
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/{TVShowId:int}")]
        public async Task<ActionResult<ApiResponseDto<TVShowDto>>> GetTVShow(int TVShowId)
        {
            var response = await mediator.Send(new GetTVShowQuery(TVShowId));
            if (response.IsSucceed)
            {
                return (response.Result is null)
                    ? NotFound(response)
                    : Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/RecommendedTVShows")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetRecommendedTVShows([FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            var query = new GetRecommendedTVShowsQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                TotalNumberOfItemsRetrieved);

            var response = await mediator.Send(query);

            if(response.IsSucceed)
            {
                return (response.Result is null)
                    ? NotFound()
                    : Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/TVShowBy")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetTVShowsBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            var response = await mediator.Send(new GetTVShowsByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId));

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
    }
}
