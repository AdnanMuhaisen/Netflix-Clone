using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetMovieQuery(int ContentId) : IRequest<MovieDto>
    {
        public readonly int contentId = ContentId;
    }
}
