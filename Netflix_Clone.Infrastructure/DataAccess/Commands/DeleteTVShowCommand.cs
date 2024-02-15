using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteTVShowCommand(int TVShowId) : IRequest<ApiResponseDto>
    {
        public readonly int tVShowId = TVShowId;
    }
}
