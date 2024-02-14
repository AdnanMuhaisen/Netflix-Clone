using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetTVShowQueryHandler : IRequestHandler<GetTVShowQuery, TVShowDto>
    {
        private readonly ILogger<GetTVShowQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public GetTVShowQueryHandler(ILogger<GetTVShowQueryHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<TVShowDto> Handle(GetTVShowQuery request, CancellationToken cancellationToken)
        {
            var targetTVShow = await applicationDbContext
                .TVShows
                .Where(x => x.Id == request.tVShowId)
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsNoTracking()
                .SingleAsync();

            var result = targetTVShow.Adapt<TVShowDto>();

            //encode the locations
            result.Location = Encoding.UTF8.GetString(Convert.FromBase64String(result.Location));
            foreach (var season in result.Seasons)
            {
                season.DirectoryName = Encoding.UTF8.GetString(Convert.FromBase64String(season.DirectoryName));
                foreach (var episode in season.Episodes)
                {
                    episode.FileName = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                                season.DirectoryName,
                                Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName)));
                }
            }

            return result;
        }
    }
}
