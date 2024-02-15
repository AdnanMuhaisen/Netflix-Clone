using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetTVShowSeasonEpisodesQueryHandler 
        : IRequestHandler<GetTVShowSeasonEpisodesQuery, ApiResponseDto>
    {
        private readonly ILogger<GetTVShowSeasonEpisodesQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetTVShowSeasonEpisodesQueryHandler(ILogger<GetTVShowSeasonEpisodesQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponseDto> Handle(GetTVShowSeasonEpisodesQuery request, CancellationToken cancellationToken)
        {
            var episodes = await applicationDbContext
                .TVShowEpisodes
                .Where(x => x.TVShowId == request.tVShowSeasonEpisodesRequestDto.TVShowId
                && x.SeasonId == request.tVShowSeasonEpisodesRequestDto.TVShowSeasonId)
                .ToListAsync();            

            if(episodes is null)
            {
                return new ApiResponseDto { Result = Enumerable.Empty<TVShowEpisodeDto>() };
            }

            foreach (var episode in episodes)
                episode.FileName = Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName));

            return new ApiResponseDto { Result = episodes.Adapt<List<TVShowEpisodeDto>>() };
        }
    }
}
