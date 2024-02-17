using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Queries
{
    public class GetTVShowSeasonsQuery(int TVShowContentId) 
        : IRequest<ApiResponseDto<IEnumerable<TVShowSeasonDto>>>
    {
        public readonly int tVShowContentId = TVShowContentId;
    }
}
