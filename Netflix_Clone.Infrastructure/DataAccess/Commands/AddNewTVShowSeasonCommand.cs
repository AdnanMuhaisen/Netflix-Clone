using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewTVShowSeasonCommand(TVShowSeasonToInsertDto tVShowSeasonToInsertDto) 
        : IRequest<ApiResponseDto>
    {
        public readonly TVShowSeasonToInsertDto tVShowSeasonToInsertDto = tVShowSeasonToInsertDto;
    }
}
