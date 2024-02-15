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
    [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME)]
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
        public async Task<ActionResult<ApiResponseDto>> GetAllTVShows()
        {
            var query = new GetAllTVShowsQuery();
            var response = await mediator.Send(query);
            return (response is null) ? BadRequest() : Ok(response);
        }

        [HttpPost]
        [Route("POST/AddNewTVShow")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> AddNewTVShow([FromBody] TVShowToInsertDto tVShowToInsertDto)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var command = new AddNewTVShowCommand(tVShowToInsertDto);
                    var result = await mediator.Send(command);
                    return Created("", result);
                }
                catch (Exception ex) 
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(new { tVShowToInsertDto, Message = "Invalid Model State" });
            }
        }

        [HttpDelete]
        [Route("DELETE/{TVShowId:int}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> DeleteTVShow([FromRoute] int TVShowId)
        {
            // there`s a cascade delete between the tbl_TVShows table and the tbl_TVShowSeasons table 
            // but to avoid the cycles or multiple cascade paths problem : i have created a trigger 
            // to delete the season episodes when the season is deleted.

            var command = new DeleteTVShowCommand(TVShowId);
            var response = await mediator.Send(command);
            return (((DeletionResultDto)response.Result).IsDeleted)
                ? NoContent()
                : BadRequest(response);
        }

        [HttpGet]
        [Route("GET/{TVShowId:int}")]
        public async Task<ActionResult<ApiResponseDto>> GetTVShow(int TVShowId)
        {
            var query = new GetTVShowQuery(TVShowId);
            var response = await mediator.Send(query);
            return (response.Result is null)
                ? NotFound(response)
                : Ok(response);
        }

        // get recommended TVShows
        // get TVShow based on filters 

        [HttpGet]
        [Route("GET/GetRecommendedTVShows")]
        public async Task<ActionResult<ApiResponseDto>> GetRecommendedTVShows([FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            var query = new GetRecommendedTVShowsQuery(
                User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                TotalNumberOfItemsRetrieved);

            var response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("GET/GetTVShowBy")]
        public async Task<ActionResult<ApiResponseDto>> GetTVShowsBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            var query = new GetTVShowsByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId);
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}
