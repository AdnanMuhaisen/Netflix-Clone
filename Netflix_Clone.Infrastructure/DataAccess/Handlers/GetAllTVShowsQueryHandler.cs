using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetAllTVShowsQueryHandler : IRequestHandler<GetAllTVShowsQuery, IEnumerable<TVShowDto>>
    {
        private readonly ILogger<GetAllTVShowsQuery> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public GetAllTVShowsQueryHandler(ILogger<GetAllTVShowsQuery> logger
            ,ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<IEnumerable<TVShowDto>> Handle(GetAllTVShowsQuery request, CancellationToken cancellationToken)
        {
            var tvShows = applicationDbContext
                .TVShows
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsNoTracking()
                .ToList();

            if(tvShows is null)
            {
                return Enumerable.Empty<TVShowDto>();
            }

            var result = tvShows.Adapt<List<TVShowDto>>();

            foreach (var tvShow in result)
            {
                tvShow.Location = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                    Encoding.UTF8.GetString(Convert.FromBase64String(tvShow.Location)));

                foreach (var season in tvShow.Seasons)
                {
                    season.DirectoryName =Encoding.UTF8.GetString(Convert.FromBase64String(season.DirectoryName));
                    foreach (var episode in season.Episodes)
                    {
                        episode.FileName = Path.Combine(tvShow.Location,
                            season.DirectoryName,
                             Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName))
                            );
                    }
                }
            }

            return result;
        }
    }
}
