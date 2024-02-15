using Castle.Core.Logging;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetRecommendedMoviesQueryHandler : IRequestHandler<GetRecommendedMoviesQuery, ApiResponseDto>
    {
        private readonly ILogger<GetRecommendedMoviesQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetRecommendedMoviesQueryHandler(ILogger<GetRecommendedMoviesQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<ApiResponseDto> Handle(GetRecommendedMoviesQuery request, CancellationToken cancellationToken)
        {
            var userHistory = await applicationDbContext
                .UsersWatchHistories
                .AsNoTracking()
                .Include(x => x.Content)
                .Where(x => x.ApplicationUserId == request.userId)
                .ToListAsync();

            userHistory = userHistory
                .DistinctBy(x => x.ContentId)
                .ToList();

            // the operation is as follows:
            // we will get the genres of the user watch history and then get 
            // the movies based on this genres 

            var preferredGenres = userHistory
                .Select(x => x.Content?.ContentGenreId)
                .Distinct();

            var recommendedMovies = Enumerable.Empty<Movie>();

            // in case of there`s no movies according to preferred genres
            if (userHistory is null || !userHistory.Any() || !preferredGenres.Any())
            {
                // return random movies
                recommendedMovies = await applicationDbContext
                    .Movies
                    .AsNoTracking()
                    .OrderBy(x => Guid.NewGuid())
                    .Take(request.totalNumberOfItemsRetrieved)
                    .ToListAsync();

                if (recommendedMovies is null)
                    recommendedMovies = new List<Movie>();

                return new ApiResponseDto
                {
                    Result = recommendedMovies.Adapt<List<MovieDto>>(),
                    Message = "There`s no movies to retrieve"
                };
            }

            recommendedMovies = await applicationDbContext
                .Movies
                .AsNoTracking()
                .Where(x => preferredGenres.Contains(x.ContentGenreId))
                .Take(request.totalNumberOfItemsRetrieved)
                .ToListAsync();

            if(recommendedMovies.Count() < request.totalNumberOfItemsRetrieved)
            {
                recommendedMovies = recommendedMovies.Union(applicationDbContext
                    .Movies
                    .AsNoTracking()
                    .Where(x => !recommendedMovies.Contains(x))
                    .OrderBy(x => Guid.NewGuid())
                    .Take(request.totalNumberOfItemsRetrieved - recommendedMovies.Count()))
                    .ToList();
            }

            var result = recommendedMovies.Adapt<List<MovieDto>>();

            return new ApiResponseDto
            {
                Result = result
            };
        }
    }
}
