using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.Common.Commands;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    //[Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class MoviesController(
        ILogger<MoviesController> logger,
        ISender sender,
        IWebHostEnvironment webHostEnvironment,
        IFileCompressor fileCompressor,
        IOptions<ContentMovieOptions> contentOptions)

        : BaseController<MoviesController>(logger)
    {
        private readonly ISender sender = sender;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly IFileCompressor fileCompressor = fileCompressor;
        private readonly IOptions<ContentMovieOptions> contentOptions = contentOptions;

        #region version_1.0

        [HttpGet]
        [Route("")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetAllMovies()
        {
            logger.LogTrace($"{nameof(GetAllMovies)} is executing");

            var response = await sender.Send(new GetAllMoviesQuery());

            return response.IsSucceed
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpPost]
        [Route("POST")]
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> AddNewMovie([FromBody] MovieToInsertDto movieToInsertDto)
        {
            logger.LogTrace($"{nameof(AddNewMovie)} is executing");

            if (ModelState.IsValid)
            {
                var response = await sender.Send(movieToInsertDto.Adapt<AddNewMovieCommand>());

                logger.LogTrace($"The {nameof(AddNewMovie)} executed successfully");

                if (response.IsSucceed && response.Result is not null)
                {
                    return Created("", response);
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
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteMovie(int ContentId)
        {
            logger.LogTrace($"Try to delete the movie with id : {ContentId}");

            var deleteMovieCommand = new DeleteMovieCommand(ContentId);

            var response = await sender.Send(deleteMovieCommand);

            if (response.IsSucceed && response.Result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("PUT")]
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> UpdateMovieInfo([FromBody] MovieDto movieDto)
        {
            var command = new UpdateMovieCommand(movieDto);

            var response = await sender.Send(command);

            if (response.IsSucceed && response.Result is not null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/{ContentId:int}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> GetMovieById([FromRoute] int ContentId)
        {
            logger.LogTrace("The get movie by Id action in started"); ;

            var response = await sender.Send(new GetMovieQuery(ContentId));

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

                    await sender.Send(addToUserHistoryCommand);
                }
            }

            if (response.IsSucceed && response.Result is not null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("POST/Download")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<DownloadMovieResponseDto>>> DownloadMovie(
            [FromBody] DownloadMovieRequestDto downloadMovieRequestDto)
        {
            logger.LogTrace("The download movie action is started");

            if (ModelState.IsValid)
            {
                var command = new DownloadMovieCommand(downloadMovieRequestDto,
                    User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                logger.LogTrace("Try to execute the download movie command");

                var response = await sender.Send(command);

                if (response.IsSucceed && response.Result is not null)
                {
                    return Created("", response);
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
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetRecommendedMovies(
            [FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            logger.LogTrace($"Try to get the recommended movies for the user");

            var query = new GetRecommendedMoviesQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                TotalNumberOfItemsRetrieved);

            var response = await sender.Send(query);

            if (response.IsSucceed && response.Result is not null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpGet]
        [Route("GET/GetMoviesBy")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<MovieDto>>>> GetMoviesBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            logger.LogTrace($"Try to get movie by filter");

            var query = new GetMoviesByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId);
            var response = await sender.Send(query);

            if (response.IsSucceed && response.Result is not null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region version_1.1

        [HttpPost]
        [Route("POST")]
        [ApiVersion("1.1")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> AddNewMovieVersion1_1([FromBody] MovieToInsertDto movieToInsertDto,
            IMoviesIndexRepository moviesIndexRepository)
        {
            logger.LogTrace($"{nameof(AddNewMovie)} is executing");

            if (ModelState.IsValid)
            {
                var response = await sender.Send(movieToInsertDto.Adapt<AddNewMovieCommand>());

                if (response.IsSucceed && response.Result is not null)
                {
                    var elsResponse = await sender.Send(new AddNewMovieDocumentCommand(response.Result));
                    //log the result

                    return Created("", response);
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
        [ApiVersion("1.1")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteMovieVersion1_1(int ContentId)
        {
            logger.LogTrace($"Try to delete the movie with id : {ContentId}");

            var deleteMovieCommand = new DeleteMovieCommand(ContentId);

            var response = await sender.Send(deleteMovieCommand);

            if (response.IsSucceed && response.Result)
            {
                var elsDeleteResponse = await sender.Send(new DeleteMovieDocumentCommand(ContentId));
                //log the response

                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("PUT")]
        [ApiVersion("1.1")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<MovieDto>>> UpdateMovieInfoVersion1_1([FromBody] MovieDto movieDto)
        {
            var command = new UpdateMovieCommand(movieDto);

            var response = await sender.Send(command);

            if (response.IsSucceed && response.Result is not null)
            {
                var updateResponse = await sender.Send(new UpdateMovieDocumentCommand(movieDto));
                //log the result

                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GET/Search")]
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
        public async Task<ActionResult<ApiResponseDto<ELSSearchResponse<MovieDocument>>>> Search([FromQuery] string searchQuery)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchQuery);

            var searchResponse = await sender.Send(new SearchByMovieTitleOrSynopsisQuery(searchQuery));

            return Ok(new ApiResponseDto<ELSSearchResponse<MovieDocument>> { IsSucceed = true, Result = searchResponse });
        }

        #endregion
    }
}
