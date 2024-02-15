using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.Checkers;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
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
        public async Task<ActionResult<ApiResponseDto>> GetAllMovies()
        {
            logger.LogTrace($"{nameof(GetAllMovies)} is executing");

            var query = new GetAllMoviesQuery();

            logger.LogTrace($"The {nameof(GetAllMoviesQuery)} is initialized");

            ApiResponseDto response = default!;

            try
            {
                response = await mediator.Send(query);

                logger.LogTrace($"The {nameof(GetAllMoviesQuery)} is executed");
            }
            catch (Exception ex)
            {
                logger.LogError($"The {nameof(GetAllMoviesQuery)} is failed due to {ex.Message}");

                return BadRequest(response);
            }

            logger.LogTrace($"{nameof(AddNewMovie)} is executed");

            return Ok(response);
        }

        [HttpPost]
        [Route("POST")]
        [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> AddNewMovie([FromBody] MovieToInsertDto movieToInsertDto)
        {
            logger.LogTrace($"{nameof(AddNewMovie)} is executing");

            if (ModelState.IsValid)
            {
                if (!System.IO.File.Exists(movieToInsertDto.Location))
                {
                    logger.LogError($"The file location {movieToInsertDto.Location} does not exists");

                    return BadRequest(movieToInsertDto);
                }

                if (!MediaFileExtensionChecker.IsValidFileExtension(Path.GetExtension(movieToInsertDto.Location)))
                {
                    logger.LogError($"The file extension {Path.GetExtension(movieToInsertDto.Location)} is not valid");

                    return BadRequest(movieToInsertDto);
                }

                // process and save the movie:
                var command = new AddNewMovieCommand(movieToInsertDto);
                 ApiResponseDto response = default!;

                try
                {
                    response = await mediator.Send(command);
                }
                catch (InvalidMappingOperationException ex)
                {
                    return BadRequest(new { movieToInsertDto, ex.Message });
                }
                catch (InsertionException ex)
                {
                    return BadRequest(new { movieToInsertDto, ex.Message });
                }

                logger.LogTrace($"The {nameof(AddNewMovie)} executed successfully");

                return Created("", response);
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
        [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> DeleteMovie(int ContentId)
        {
            var deleteMovieCommand = new DeleteMovieCommand(ContentId);

            ApiResponseDto response = default!;
            try
            {
                response = await mediator.Send(deleteMovieCommand);
            }
            catch (Exception ex)
            {
                return BadRequest($"EntityNotFound {ex.Message}");
            }

            if (!((bool)(response.Result)))
                return BadRequest("The movie is failed to delete");

            return NoContent();
        }

        [HttpPut]
        [Route("PUT")]
        [Authorize(AuthenticationSchemes =BEARER_AUTHENTICATION_SCHEME,Roles =ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> UpdateMovieInfo([FromBody] MovieDto movieDto)
        {
            var command = new UpdateMovieCommand(movieDto);

            try
            {
                await mediator.Send(command);
            }
            catch(Exception ex)
            {
                return BadRequest(new { movieDto, ex.Message });
            }
            return NoContent(); 
        }

        [HttpGet]
        [Route("GET/{ContentId:int}")]
        public async Task<ActionResult<ApiResponseDto>> GetMovie([FromRoute] int ContentId)
        {
            logger.LogTrace("The get movie action in started");

            var query = new GetMovieQuery(ContentId);

            try
            {
                var response = await mediator.Send(query);

                logger.LogTrace("The move with id : {id} is retrieved successfully",
                    ((MovieDto)response.Result).Id);

                //add to user history if the user role is user:
                if(User.Claims.First(c=>c.Type == ClaimTypes.Role).Value == USER_ROLE)
                {
                    var addToUserHistoryCommand = new AddToUserWatchHistoryCommand(new AddToUserWatchHistoryRequestDto
                    {
                        ApplicationUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                        ContentId = ContentId
                    });

                    await mediator.Send(addToUserHistoryCommand);
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError("An exception is thrown while trying to get the movie with id : {id} because this exception {message}", ContentId, ex.Message);

                return NotFound(ContentId);
            }
        }

        //test
        [HttpPost]
        [Route("POST/Download")]
        public async Task<ActionResult<ApiResponseDto>> DownloadMovie([FromBody] DownloadMovieRequestDto downloadMovieRequestDto)
        {
            logger.LogTrace("The download movie action is started");

            if (ModelState.IsValid)
            {
                var command = new DownloadMovieCommand(downloadMovieRequestDto,
                    User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                try
                {
                    logger.LogTrace("Try to execute the download movie command");

                    var downloadMovieResult = await mediator.Send(command);

                    ArgumentNullException.ThrowIfNull(downloadMovieResult);

                    logger.LogTrace("The download movie command is executed successfully");

                    return Ok(downloadMovieResult);
                }
                catch(Exception ex)
                {
                    logger.LogError("An error occurred while trying to download the movie because this exception : {message}", ex.Message);

                    return BadRequest(new ApiResponseDto
                    {
                        Result = new DownloadMovieResponseDto { IsDownloaded = false },
                        Message = ex.Message
                    });
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("GET/GetRecommendedMovies")]
        public async Task<ActionResult<ApiResponseDto>> GetRecommendedMovies(
            [FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            var query = new GetRecommendedMoviesQuery(
                User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                TotalNumberOfItemsRetrieved);

            var response = await mediator.Send(query);

            return Ok(response);
        }


        [HttpGet]
        [Route("GET/GetMoviesBy")]
        public async Task<ActionResult<ApiResponseDto>> GetMoviesBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            var query = new GetMoviesByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId);
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}
