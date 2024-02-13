using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewTVShowSeasonCommand(TVShowSeasonToInsertDto tVShowSeasonToInsertDto) : IRequest<TVShowSeasonDto>
    {
        public readonly TVShowSeasonToInsertDto tVShowSeasonToInsertDto = tVShowSeasonToInsertDto;
    }
}
