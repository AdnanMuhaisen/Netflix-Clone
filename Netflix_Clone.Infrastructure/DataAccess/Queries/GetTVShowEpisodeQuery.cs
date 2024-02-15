using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowEpisodeQuery(TVShowEpisodeRequestDto tVShowEpisodeRequestDto) : IRequest<ApiResponseDto>
    {
        public readonly TVShowEpisodeRequestDto tVShowEpisodeRequestDto = tVShowEpisodeRequestDto;
    }
}
