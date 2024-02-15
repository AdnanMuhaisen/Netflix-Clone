using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowSeasonsQuery(int TVShowContentId) : IRequest<ApiResponseDto>
    {
        public readonly int tVShowContentId = TVShowContentId;
    }
}
