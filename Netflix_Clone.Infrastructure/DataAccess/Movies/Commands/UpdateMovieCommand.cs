using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Commands
{
    public class UpdateMovieCommand(MovieDto movieDto) : IRequest<ApiResponseDto<MovieDto>>
    {
        public readonly MovieDto movieDto = movieDto;
    }
}
