using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewTVShowEpisodeCommand(TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto) : IRequest<TVShowEpisodeDto>
    {
        public TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto { get; } = TVShowEpisodeToInsertDto;
    }
}
