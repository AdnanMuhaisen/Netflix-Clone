using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands
{
    public class DownloadTVShowEpisodeCommand(DownloadEpisodeRequestDto downloadEpisodeRequestDto, string UserId)
        : IRequest<ApiResponseDto>
    {
        public readonly DownloadEpisodeRequestDto downloadEpisodeRequestDto = downloadEpisodeRequestDto;
        public readonly string userId = UserId;
    }
}
