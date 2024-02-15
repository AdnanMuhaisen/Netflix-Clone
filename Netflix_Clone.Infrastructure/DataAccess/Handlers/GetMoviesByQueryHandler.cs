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
    public class GetMoviesByQueryHandler : IRequestHandler<GetMoviesByQuery, ApiResponseDto>
    {
        private readonly ILogger<GetMoviesByQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetMoviesByQueryHandler(ILogger<GetMoviesByQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponseDto> Handle(GetMoviesByQuery request, CancellationToken cancellationToken)
        {
            Func<Movie, bool> moviesFilter = (movie) =>
            {
                bool result = true;

                result = result && (request.genreId is not null) ? movie.ContentGenreId == request.genreId : result;
                result = result && (request.releaseYear is not null) ? movie.ReleaseYear == request.releaseYear : result;
                result = result && (request.minimumAgeToWatch is not null) ? movie.MinimumAgeToWatch == request.minimumAgeToWatch : result;
                result = result && (request.languageId is not null) ? movie.LanguageId == request.languageId : result;
                result = result && (request.directorId is not null) ? movie.DirectorId == request.directorId : result;

                return result;
            };

            var movies = applicationDbContext
                .Movies
                .AsNoTracking()
                .Where(moviesFilter)
                .ToList();

            if(movies is null)
            {
                movies = new List<Movie>();
            }

            return new ApiResponseDto
            {
                Result = movies.Adapt<List<MovieDto>>()
            };
        }
    }
}
