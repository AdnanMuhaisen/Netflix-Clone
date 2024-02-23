using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class GetMovieQueryHandler(ILogger<GetMovieQuery> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentMovieOptions> options) 
        : IRequestHandler<GetMovieQuery, ApiResponseDto<MovieDto>>
    {
        private readonly ILogger<GetMovieQuery> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentMovieOptions> options = options;

        public async Task<ApiResponseDto<MovieDto>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The get movie query handler is start to execute");

            var targetMovieToRetrieve = await applicationDbContext
                .Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.contentId);

            if(targetMovieToRetrieve is null)
            {
                logger.LogInformation($"Can not find the movie with id : {request.contentId}");

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = "Can not find the target movie"
                };
            }

            logger.LogTrace("The movie is retrieved successfully");

            //decode the movie location
            //targetMovieToRetrieve.Location = Path.Combine(options.Value.TargetDirectoryToSaveTo, targetMovieToRetrieve.Location);
            targetMovieToRetrieve.Location = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToRetrieve.Location)));

            return new ApiResponseDto<MovieDto>
            { 
                Result = targetMovieToRetrieve.Adapt<MovieDto>(),
                IsSucceed = true
            };
        }
    }
}
