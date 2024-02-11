using Azure.Core;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.Checkers;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : BaseController<MoviesController>
    {
        private readonly IMediator mediator;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFileCompressor fileCompressor;
        private readonly IOptions<ContentOptions> contentOptions;

        public MoviesController(ILogger<MoviesController> logger,
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment,
            IFileCompressor fileCompressor,
            IOptions<ContentOptions> contentOptions)
            : base(logger)
        {
            this.mediator = mediator;
            this.webHostEnvironment = webHostEnvironment;
            this.fileCompressor = fileCompressor;
            this.contentOptions = contentOptions;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
        {
            logger.LogTrace($"{nameof(GetAllMovies)} is executing");

            var query = new GetAllMoviesQuery();

            logger.LogTrace($"The {nameof(GetAllMoviesQuery)} is initialized");

            var queryResult = Enumerable.Empty<MovieDto>();

            try
            {
                 queryResult = await mediator.Send(query);

                logger.LogTrace($"The {nameof(GetAllMoviesQuery)} is executed");
            }
            catch (Exception ex)
            {
                logger.LogError($"The {nameof(GetAllMoviesQuery)} is failed due to {ex.Message}");

                return BadRequest(queryResult);
            }

            logger.LogTrace($"{nameof(AddNewMovie)} is executed");

            return Ok(queryResult);
        }

        [HttpPost]
        [Route("POST")]
        public async Task<ActionResult<MovieDto>> AddNewMovie([FromBody] MovieToInsertDto movieToInsertDto)
        {
            logger.LogTrace($"{nameof(AddNewMovie)} is executing");

            if (ModelState.IsValid)
            {
                if(!System.IO.File.Exists(movieToInsertDto.Location))
                {
                    logger.LogError($"The file location {movieToInsertDto.Location} does not exists");

                    return BadRequest(movieToInsertDto);
                }

                if(!MediaFileExtensionChecker.IsValidFileExtension(Path.GetExtension(movieToInsertDto.Location)))
                {
                    logger.LogError($"The file extension {Path.GetExtension(movieToInsertDto.Location)} is not valid");

                    return BadRequest(movieToInsertDto);
                }

                // process and save the movie:
                var command = new AddNewMovieCommand(movieToInsertDto);
                MovieDto result = default!;

                try
                {
                     result = await mediator.Send(command);
                }
                catch (InvalidMappingOperationException ex)
                {
                    return BadRequest(new { movieToInsertDto, ex.Message });
                }
                catch(InsertionException ex)
                {
                    return BadRequest(new { movieToInsertDto, ex.Message });
                }

                logger.LogTrace($"The {nameof(AddNewMovie)} executed successfully");

                return Ok(result);
            }
            else
            {
                logger.LogTrace($"The model state is not valid when try to {nameof(AddNewMovie)} because\n" +
                    $"{ModelState.Values}");

                return BadRequest(default!);
            }
        }


    }
}
