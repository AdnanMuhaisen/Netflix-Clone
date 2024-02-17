using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Queries
{
    public class GetTVShowSeasonEpisodesQuery(TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto)
        : IRequest<ApiResponseDto<IEnumerable<TVShowEpisodeDto>>>
    {
        public readonly TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto = tVShowSeasonEpisodesRequestDto;
    }
}
