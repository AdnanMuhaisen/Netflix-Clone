using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Security.Claims;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
    public class TVShowsController(
        ILogger<TVShowsController> logger,
        ISender sender)
        
        : BaseController<TVShowsController>(logger)
    {
        private readonly ISender sender = sender;

        #region version_1.0

        [HttpGet]
        [Route("GET")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetAllTVShows()
        {
            logger.LogTrace($"Try to get all the available tv shows");

            var response = await sender.Send(new GetAllTVShowsQuery());

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
        [Route("POST/AddNewTVShow")]
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<TVShowDto>>> AddNewTVShow([FromBody] TVShowToInsertDto tVShowToInsertDto)
        {
            if (ModelState.IsValid)
            {
                logger.LogTrace($"Try to add the tv show with title : {tVShowToInsertDto.Title}");

                var response = await sender.Send(tVShowToInsertDto.Adapt<AddNewTVShowCommand>());

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
        [ApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteTVShow([FromRoute] int TVShowId)
        {
            // there`s a cascade delete between the tbl_TVShows table and the tbl_TVShowSeasons table 
            // but to avoid the cycles or multiple cascade paths problem : i have created a trigger 
            // to delete the season episodes when the season is deleted.
            logger.LogTrace($"Try to delete the tv show with id : {TVShowId}");

            var response = await sender.Send(new DeleteTVShowCommand(TVShowId));

            if (response.IsSucceed && response.Result.IsDeleted)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GET/{TVShowId:int}")]
        public async Task<ActionResult<ApiResponseDto<TVShowDto>>> GetTVShowById(int TVShowId)
        {
            var response = await sender.Send(new GetTVShowQuery(TVShowId));
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
        [ApiVersion("1.0")]
        [Route("GET/RecommendedTVShows")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetRecommendedTVShows(
            [FromQuery] int TotalNumberOfItemsRetrieved = 10)
        {
            var query = new GetRecommendedTVShowsQuery(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
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
        [ApiVersion("1.0")]
        [Route("GET/TVShowBy")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TVShowDto>>>> GetTVShowsBy(
            [FromQuery] int? GenreId = default,
            [FromQuery] int? ReleaseYear = default,
            [FromQuery] int? MinimumAgeToWatch = default,
            [FromQuery] int? LanguageId = default,
            [FromQuery] int? DirectorId = default)
        {
            logger.LogTrace($"Try to get the tv shows by filtering them");

            var response = await sender.Send(new GetTVShowsByQuery(GenreId, ReleaseYear, MinimumAgeToWatch, LanguageId, DirectorId));

            if (response.IsSucceed && response.Result is not null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        #endregion

        #region version_1.1

        [HttpPost]
        [Route("POST/AddNewTVShow")]
        [ApiVersion("1.1")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<TVShowDto>>> AddNewTVShowVersion1_1([FromBody] TVShowToInsertDto tVShowToInsertDto)
        {
            if (ModelState.IsValid)
            {
                logger.LogTrace($"Try to add the tv show with title : {tVShowToInsertDto.Title}");

                var response = await sender.Send(tVShowToInsertDto.Adapt<AddNewTVShowCommand>());

                if (response.IsSucceed && response.Result is not null)
                {
                    var elsResponse = await sender.Send(new AddNewTVShowDocumentCommand(response.Result));
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
        [ApiVersion("1.1")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<DeletionResultDto>>> DeleteTVShowVersion1_1([FromRoute] int TVShowId)
        {
            // there`s a cascade delete between the tbl_TVShows table and the tbl_TVShowSeasons table 
            // but to avoid the cycles or multiple cascade paths problem : i have created a trigger 
            // to delete the season episodes when the season is deleted.
            logger.LogTrace($"Try to delete the tv show with id : {TVShowId}");

            var response = await sender.Send(new DeleteTVShowCommand(TVShowId));

            if (response.IsSucceed && response.Result.IsDeleted)
            {
                var elsResponse = await sender.Send(new DeleteTVShowDocumentCommand(TVShowId));
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
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME)]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<ELSSearchResponse<TVShowDocument>>>> Search([FromQuery] string searchQuery)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchQuery);

            var searchResponse = await sender.Send(new SearchByTVShowTitleOrSynopsisQuery(searchQuery));

            return Ok(new ApiResponseDto<ELSSearchResponse<TVShowDocument>> { IsSucceed = true, Result = searchResponse });
        }

        #endregion
    }
}
