using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class UpdateMovieCommand(MovieDto movieDto) : IRequest<MovieDto>
    {
        public readonly MovieDto movieDto = movieDto;
    }
}
