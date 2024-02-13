
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<TVShowEpisodeDto>>> GetAllTVShowSeasonEpisodes(
            [FromBody] TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto)
        {
            if (ModelState.IsValid)
            {
                var query = new GetTVShowSeasonEpisodesQuery(tVShowSeasonEpisodesRequestDto);
                var result = await mediator.Send(query);
                return (result is null) ? BadRequest() : Ok(result);
            }
            else 
            {
                return BadRequest("Invalid Model State");
            }
        }

        [HttpPost]
        [Route("POST/AddTVShowSeasonEpisode")]
        public async Task<ActionResult<TVShowEpisodeDto>> AddNewSeasonEpisode([FromBody] TVShowEpisodeToInsertDto tVShowEpisodeToInsert)
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


    }
}
