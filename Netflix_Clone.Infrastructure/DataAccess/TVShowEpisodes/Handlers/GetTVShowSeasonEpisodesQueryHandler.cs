using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Handlers
{
    public class GetTVShowSeasonEpisodesQueryHandler(ILogger<GetTVShowSeasonEpisodesQueryHandler> logger,
        ApplicationDbContext applicationDbContext)
                : IRequestHandler<GetTVShowSeasonEpisodesQuery, ApiResponseDto<IEnumerable<TVShowEpisodeDto>>>
    {
        private readonly ILogger<GetTVShowSeasonEpisodesQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<IEnumerable<TVShowEpisodeDto>>> Handle(GetTVShowSeasonEpisodesQuery request, CancellationToken cancellationToken)
        {
            var episodes = await applicationDbContext
                .TVShowEpisodes
                .AsNoTracking()
                .Where(x => x.TVShowId == request.tVShowSeasonEpisodesRequestDto.TVShowId
                && x.SeasonId == request.tVShowSeasonEpisodesRequestDto.TVShowSeasonId)
                .ToListAsync() ?? [];

            foreach (var episode in episodes)
                episode.FileName = Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName));

            return new ApiResponseDto<IEnumerable<TVShowEpisodeDto>>
            {
                Result = episodes.Adapt<List<TVShowEpisodeDto>>(),
                IsSucceed = true
            };
        }
    }
}
