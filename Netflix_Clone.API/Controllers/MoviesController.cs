using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.Checkers;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Common.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class MoviesController : BaseController<MoviesController>
    {
        private readonly IMediator mediator;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFileCompressor fileCompressor;
        private readonly IOptions<ContentMovieOptions> contentOptions;

        public MoviesController(ILogger<MoviesController> logger,
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment,
            IFileCompressor fileCompressor,
            IOptions<ContentMovieOptions> contentOptions)
            : base(logger)
        {
            this.mediator = mediator;
            this.webHostEnvironment = webHostEnvironment;
            this.fileCompressor = fileCompressor;
            this.contentOptions = contentOptions;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetAllMovies()
        {
            logger.LogTrace($"{nameof(GetAllMovies)} is executing");

            var response = await mediator.Send(new GetAllMoviesQuery());

            return (response.IsSucceed)
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpPost]
        [Route("POST")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> AddNewMovie([FromBody] MovieToInsertDto movieToInsertDto)
        {
            logger.LogTrace($"{nameof(AddNewMovie)} is executing");

            if (ModelState.IsValid)
            {
                var response = await mediator.Send(movieToInsertDto.Adapt<AddNewMovieCommand>());
                
                logger.LogTrace($"The {nameof(AddNewMovie)} executed successfully");

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
                logger.LogTrace($"The model state is not valid when try to {nameof(AddNewMovie)} because\n" +
                    $"{ModelState.Values}");

                return BadRequest(default!);
            }
        }

        [HttpDelete]
        [Route("DELETE/{ContentId:int}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteMovie(int ContentId)
        {
            var deleteMovieCommand = new DeleteMovieCommand(ContentId);

            var response = await mediator.Send(deleteMovieCommand);
            
            if(response.IsSucceed)
            {
                return (response.Result)
                    ? NoContent()
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("PUT")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> UpdateMovieInfo([FromBody] MovieDto movieDto)
        {
            var command = new UpdateMovieCommand(movieDto);

            var response = await mediator.Send(command);

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? NoContent()
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> GetMovie([FromRoute] int ContentId)
        {
            logger.LogTrace("The get movie action in started");;

            var response = await mediator.Send(new GetMovieQuery(ContentId));

            logger.LogTrace("The move with id : {id} is retrieved successfully",
                ((MovieDto)response.Result).Id);

            //add to user history if the user role is user:
            if (response.IsSucceed && response.Result is not null)
            {
                if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == USER_ROLE)
                {
                    var addToUserHistoryCommand = new AddToUserWatchHistoryCommand(new AddToUserWatchHistoryRequestDto
                    {
                        ApplicationUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                        ContentId = ContentId
                    });

                    await mediator.Send(addToUserHistoryCommand);
                }                
            }

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? Ok(response)
                    : NotFound(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("POST/Download")]
        public async Task<ActionResult<ApiResponseDto<DownloadMovieResponseDto>>> DownloadMovie([FromBody] DownloadMovieRequestDto downloadMovieRequestDto)
        {
            logger.LogTrace("The download movie action is started");

            if (ModelState.IsValid)
            {
                var command = new DownloadMovieCommand(downloadMovieRequestDto,
                    User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                logger.LogTrace("Try to execute the download movie command");

                var response = await mediator.Send(command);

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
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("GET/GetRecommendedMovies")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetRecommendedMovies([FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            var query = new GetRecommendedMoviesQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                TotalNumberOfItemsRetrieved);

            var response = await mediator.Send(query);

            if (response.IsSucceed)
            {
                return (response.Result is not null)
                    ? Ok(response)
                    : NotFound(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpGet]
        [Route("GET/GetMoviesBy")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetMoviesBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            var query = new GetMoviesByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId);
            var response = await mediator.Send(query);

            if (response.IsSucceed)
            {
                return(response.Result is not null)
                    ? Ok(response)
                    : NotFound(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
