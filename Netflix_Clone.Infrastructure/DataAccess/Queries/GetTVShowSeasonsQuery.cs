using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetTVShowSeasonsQuery(int TVShowContentId) : IRequest<IEnumerable<TVShowSeasonDto>>
    {
        public readonly int tVShowContentId = TVShowContentId;
    }
}
