using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<TVShowDto>>> GetAllTVShows()
        {
            var query = new GetAllTVShowsQuery();
            var result = await mediator.Send(query);
            return (result is null) ? BadRequest() : Ok(result);
        }

        [HttpPost]
        [Route("POST/AddNewTVShow")]
        public async Task<ActionResult<TVShowDto>> AddNewTVShow([FromBody] TVShowToInsertDto tVShowToInsertDto)
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
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(new { tVShowToInsertDto, Message = "Invalid Model State" });
            }
        }





    }
}
