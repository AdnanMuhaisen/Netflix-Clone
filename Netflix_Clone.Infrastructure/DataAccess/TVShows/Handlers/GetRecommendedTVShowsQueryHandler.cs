using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Queries;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Handlers
{
    public class GetRecommendedTVShowsQueryHandler(ILogger<GetRecommendedTVShowsQueryHandler> logger,
        ApplicationDbContext applicationDbContext) : IRequestHandler<GetRecommendedTVShowsQuery, ApiResponseDto>
    {
        private readonly ILogger<GetRecommendedTVShowsQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto> Handle(GetRecommendedTVShowsQuery request, CancellationToken cancellationToken)
        {
            var userHistory = await applicationDbContext
                .UsersWatchHistories
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == request.userId)
                .Include(x => x.Content)
                .AsSplitQuery()
                .ToListAsync();

            userHistory = userHistory
                .DistinctBy(x => x.ContentId)
                .ToList();

            // the operation is as follows:
            // we will get the genres of the user watch history and then get 
            // the TVShows based on this genres 

            var preferredGenres = userHistory
                .Select(x => x.Content?.ContentGenreId)
                .Distinct();

            var recommendedTvShows = Enumerable.Empty<TVShow>();

            // in case of there`s no movies according to preferred genres
            if (userHistory is null || !userHistory.Any() || !preferredGenres.Any())
            {
                // return random movies
                recommendedTvShows = await applicationDbContext
                    .TVShows
                    .AsNoTracking()
                    .OrderBy(x => Guid.NewGuid())
                    .Take(request.totalNumberOfItemsRetrieved)
                    .ToListAsync();

                recommendedTvShows ??= new List<TVShow>();

                return new ApiResponseDto
                {
                    Result = recommendedTvShows.Adapt<List<TVShowDto>>(),
                    Message = "There`s no TV Shows to retrieve"
                };
            }

            recommendedTvShows = await applicationDbContext
                    .TVShows
                    .AsNoTracking()
                    .Where(x => preferredGenres.Contains(x.ContentGenreId))
                    .Take(request.totalNumberOfItemsRetrieved)
                    .ToListAsync();

            if (recommendedTvShows.Count() < request.totalNumberOfItemsRetrieved)
            {
                recommendedTvShows = recommendedTvShows.Union(applicationDbContext
                    .TVShows
                    .AsNoTracking()
                    .Where(x => !recommendedTvShows.Contains(x))
                    .OrderBy(x => Guid.NewGuid())
                    .Take(request.totalNumberOfItemsRetrieved - recommendedTvShows.Count()))
                    .ToList();
            }

            var result = recommendedTvShows.Adapt<List<TVShowDto>>();

            return new ApiResponseDto
            {
                Result = result
            };
        }
    }
}
