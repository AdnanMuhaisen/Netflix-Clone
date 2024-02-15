using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewMovieCommand(MovieToInsertDto movieToInsertDto) : IRequest<ApiResponseDto>
    {
        public readonly MovieToInsertDto movieToInsertDto = movieToInsertDto;
    }
}
