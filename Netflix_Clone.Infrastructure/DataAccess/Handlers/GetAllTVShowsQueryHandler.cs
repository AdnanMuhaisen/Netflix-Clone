using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetAllTVShowsQueryHandler : IRequestHandler<GetAllTVShowsQuery, IEnumerable<TVShowDto>>
    {
        private readonly ILogger<GetAllTVShowsQuery> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetAllTVShowsQueryHandler(ILogger<GetAllTVShowsQuery> logger
            ,ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<IEnumerable<TVShowDto>> Handle(GetAllTVShowsQuery request, CancellationToken cancellationToken)
        {
            var tvShows = applicationDbContext
                .TVShows
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsNoTracking();

            if(tvShows is null)
            {
                return Enumerable.Empty<TVShowDto>();
            }

            var result = tvShows.ProjectToType<TVShowDto>();

            return await result.ToListAsync();
        }
    }
}
