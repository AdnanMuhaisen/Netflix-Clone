using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;
using Netflix_Clone.Shared.DTOs;
using System.Linq.Expressions;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class GetMoviesByQueryHandler(ILogger<GetMoviesByQueryHandler> logger,
        ApplicationDbContext applicationDbContext)
        : IRequestHandler<GetMoviesByQuery, ApiResponseDto>
    {
        private readonly ILogger<GetMoviesByQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public Task<ApiResponseDto> Handle(GetMoviesByQuery request, CancellationToken cancellationToken)
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

            movies ??= [];

            return Task.FromResult(new ApiResponseDto
            {
                Result = movies.Adapt<List<MovieDto>>()
            });
        }
    }
}
