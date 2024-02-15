using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, ApiResponseDto>
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<GetAllMoviesQueryHandler> logger;

        public GetAllMoviesQueryHandler(
            ApplicationDbContext applicationDbContext,
            ILogger<GetAllMoviesQueryHandler> logger
            )
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }


        public async Task<ApiResponseDto> Handle(GetAllMoviesQuery request,
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
                    Location = Encoding.UTF8.GetString(Convert.FromBase64String(m.Location))
                })
                .ToListAsync();

            logger.LogTrace($"The movies are retrieved from the database");

            return new ApiResponseDto { Result = movies };
        }
    }
}
