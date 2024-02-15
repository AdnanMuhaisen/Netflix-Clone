using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowQuery(int TVShowId) : IRequest<ApiResponseDto>
    {
        public readonly int tVShowId = TVShowId;
    }
}
