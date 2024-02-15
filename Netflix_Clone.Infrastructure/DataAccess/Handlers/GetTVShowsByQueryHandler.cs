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
    public class GetTVShowsByQueryHandler : IRequestHandler<GetTVShowsByQuery, ApiResponseDto>
    {
        private readonly ILogger<GetTVShowsByQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetTVShowsByQueryHandler(ILogger<GetTVShowsByQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponseDto> Handle(GetTVShowsByQuery request, CancellationToken cancellationToken)
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
                .ToList();

            if (tvShows is null)
            {
                tvShows = new List<TVShow>();
            }

            return new ApiResponseDto
            {
                Result = tvShows.Adapt<List<TVShowDto>>()
            };
        }
    }
}
