using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Handlers
{
    public class GetTVShowSeasonsQueryHandler(ILogger<GetTVShowSeasonsQueryHandler> logger,
        ApplicationDbContext applicationDbContext) : IRequestHandler<GetTVShowSeasonsQuery, ApiResponseDto>
    {
        private readonly ILogger<GetTVShowSeasonsQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto> Handle(GetTVShowSeasonsQuery request, CancellationToken cancellationToken)
        {
            var tvShowSeasons = await applicationDbContext
                .TVShowsSeasons
                .AsNoTracking()
                .Where(x => x.TVShowId == request.tVShowContentId)
                .ToListAsync();

            if (tvShowSeasons is null)
            {
                return new ApiResponseDto { Result = Enumerable.Empty<TVShowSeasonDto>() };
            }

            var result = tvShowSeasons.Adapt<List<TVShowSeasonDto>>();

            foreach (var season in result)
            {
                season.DirectoryName = Encoding.UTF8.GetString(Convert.FromBase64String(season.DirectoryName));
                foreach (var episode in season.Episodes)
                {
                    episode.FileName = Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName));
                }
            }

            return new ApiResponseDto { Result = result };
        }
    }
}
