using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME)]
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
        public async Task<ActionResult<ApiResponseDto>> GetTVShowSeasons(int TVShowContentId)
        {
            var query = new GetTVShowSeasonsQuery(TVShowContentId);
            var response = await mediator.Send(query);
            return (response is null) ? BadRequest(response) : Ok(response);
        }

        [HttpPost]
        [Route("POST/AddNewSeasonForTVShow")]
        [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> AddNewSeasonForTVShow([FromBody] TVShowSeasonToInsertDto tVShowSeasonToInsertDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new AddNewTVShowSeasonCommand(tVShowSeasonToInsertDto);
                    var response = await mediator.Send(command);
                    return Created("", response);
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
        [Route("DELETE/TVShowSeason")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> DeleteTVShowSeason([FromBody] DeleteTVShowSeasonRequestDto deleteTVShowSeasonRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new DeleteTVShowSeasonCommand(deleteTVShowSeasonRequestDto);
                var response = await mediator.Send(command);
                return (((DeletionResultDto)response.Result).IsDeleted) ? NoContent() : BadRequest(response);   
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }
    }
}
