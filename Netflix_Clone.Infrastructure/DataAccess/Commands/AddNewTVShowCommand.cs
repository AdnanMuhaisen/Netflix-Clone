using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewTVShowCommand(TVShowToInsertDto tVShowToInsertDto) : IRequest<ApiResponseDto>
    {
        public readonly TVShowToInsertDto tVShowToInsertDto = tVShowToInsertDto;
    }
}
