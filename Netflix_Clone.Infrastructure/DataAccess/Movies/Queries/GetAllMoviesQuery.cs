using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Queries
{
    public class GetAllMoviesQuery : IRequest<ApiResponseDto<IEnumerable<MovieDto>>>
    {
    }
}
