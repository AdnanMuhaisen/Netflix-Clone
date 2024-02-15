using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DownloadTVShowEpisodeCommand(DownloadEpisodeRequestDto downloadEpisodeRequestDto, string UserId)
        : IRequest<ApiResponseDto>
    {
        public readonly DownloadEpisodeRequestDto downloadEpisodeRequestDto = downloadEpisodeRequestDto;
        public readonly string userId = UserId;
    }
}
