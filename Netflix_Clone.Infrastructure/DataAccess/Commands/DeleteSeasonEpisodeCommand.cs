using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteSeasonEpisodeCommand(TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto) : IRequest<DeletionResultDto>
    {
        public readonly TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto = tVShowSeasonEpisodeToDeleteDto;
    }
}
