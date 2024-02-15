using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowSeasonEpisodesQuery(TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto)
        : IRequest<ApiResponseDto>
    {
        public readonly TVShowSeasonEpisodesRequestDto tVShowSeasonEpisodesRequestDto = tVShowSeasonEpisodesRequestDto;
    }
}
