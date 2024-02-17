using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands
{
    public class DeleteTVShowCommand(int TVShowId) : IRequest<ApiResponseDto>
    {
        public readonly int tVShowId = TVShowId;
    }
}
