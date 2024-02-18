using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Queries;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Handlers
{
    public class GetTVShowsByQueryHandler(ILogger<GetTVShowsByQueryHandler> logger,
        ApplicationDbContext applicationDbContext) : IRequestHandler<GetTVShowsByQuery, ApiResponseDto<IEnumerable<TVShowDto>>>
    {
        private readonly ILogger<GetTVShowsByQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<IEnumerable<TVShowDto>>> Handle(GetTVShowsByQuery request, CancellationToken cancellationToken)
        {
            Func<TVShow, bool> tvShowsFilter = (tvShow) =>
            {
                bool result = true;

                result = result && (request.genreId is not null) ? tvShow.ContentGenreId == request.genreId : result;
                result = result && (request.releaseYear is not null) ? tvShow.ReleaseYear == request.releaseYear : result;
                result = result && (request.minimumAgeToWatch is not null) ? tvShow.MinimumAgeToWatch == request.minimumAgeToWatch : result;
                result = result && (request.languageId is not null) ? tvShow.LanguageId == request.languageId : result;
                result = result && (request.directorId is not null) ? tvShow.DirectorId == request.directorId : result;

                return result;
            };

            var tvShows = applicationDbContext
                .TVShows
                .AsNoTracking()
                .Where(tvShowsFilter)
                .ToList() ?? [];

            return new ApiResponseDto<IEnumerable<TVShowDto>>
            {
                Result = tvShows.Adapt<List<TVShowDto>>(),
                IsSucceed = true
            };
        }
    }
}
