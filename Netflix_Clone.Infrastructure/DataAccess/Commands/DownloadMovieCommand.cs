using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DownloadMovieCommand(DownloadMovieRequestDto downloadMovieRequestDto) : IRequest<DownloadMovieResponseDto>
    {
        public readonly DownloadMovieRequestDto downloadMovieRequestDto = downloadMovieRequestDto;
    }
}
