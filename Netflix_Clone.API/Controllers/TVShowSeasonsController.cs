using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<TVShowSeasonDto>>> GetTVShowSeasons(int TVShowContentId)
        {
            var query = new GetTVShowSeasonsQuery(TVShowContentId);
            var result = await mediator.Send(query);
            return (result is null) ? BadRequest(result) : Ok(result);
        }

        [HttpPost]
        [Route("POST/AddNewSeasonForTVShow")]
        public async Task<ActionResult<TVShowSeasonDto>> AddNewSeasonForTVShow([FromBody] TVShowSeasonToInsertDto tVShowSeasonToInsertDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new AddNewTVShowSeasonCommand(tVShowSeasonToInsertDto);
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
        [Route("DELETE/DeleteTVShowSeason")]
        public async Task<ActionResult<DeletionResultDto>> DeleteTVShowSeason([FromBody] DeleteTVShowSeasonRequestDto deleteTVShowSeasonRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new DeleteTVShowSeasonCommand(deleteTVShowSeasonRequestDto);
                var result = await mediator.Send(command);
                return (result.IsDeleted) ? NoContent() : BadRequest(result);   
            }
            else
            {
                return BadRequest("Invalid Model State");
            }
        }


    }
}
