using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DownloadMovieCommand(DownloadMovieRequestDto downloadMovieRequestDto,string UserId)
        : IRequest<ApiResponseDto>
    {
        public readonly DownloadMovieRequestDto downloadMovieRequestDto = downloadMovieRequestDto;
        public readonly string userId = UserId;
    }
}
