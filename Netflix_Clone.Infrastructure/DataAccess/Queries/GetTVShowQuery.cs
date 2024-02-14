using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowQuery(int TVShowId) : IRequest<TVShowDto>
    {
        public readonly int tVShowId = TVShowId;
    }
}
