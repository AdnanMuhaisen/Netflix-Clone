using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class GetAllMoviesQueryHandler(
        ApplicationDbContext applicationDbContext,
        ILogger<GetAllMoviesQueryHandler> logger)
        : IRequestHandler<GetAllMoviesQuery, ApiResponseDto<IEnumerable<MovieDto>>>
    {
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly ILogger<GetAllMoviesQueryHandler> logger = logger;

        public async Task<ApiResponseDto<IEnumerable<MovieDto>>> Handle(GetAllMoviesQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("The Get All Movies handler is started");

            var movies = await applicationDbContext
                .Movies
                .AsNoTracking()
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    MinimumAgeToWatch = m.MinimumAgeToWatch,
                    Synopsis = m.Synopsis,
                    LengthInMinutes = m.LengthInMinutes,
                    LanguageId = m.LanguageId,
                    ContentGenreId = m.ContentGenreId,
                    DirectorId = m.DirectorId,
                    Location = m.Location
                })
                .ToListAsync() ?? [];

            logger.LogTrace($"The movies are retrieved from the database");

            return new ApiResponseDto<IEnumerable<MovieDto>> 
            { 
                Result = movies,
                IsSucceed = true
            };
        }
    }
}
