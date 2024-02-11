using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewMovieCommand : IRequest<MovieDto>
    {
        public readonly MovieToInsertDto movieToInsertDto;


        public AddNewMovieCommand(MovieToInsertDto movieToInsertDto)
        {
            this.movieToInsertDto = movieToInsertDto;
        }
    }
}
