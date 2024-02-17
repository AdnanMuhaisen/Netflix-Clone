using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Handlers
{
    public class GetTVShowQueryHandler(ILogger<GetTVShowQueryHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<GetTVShowQuery, ApiResponseDto<TVShowDto>>
    {
        private readonly ILogger<GetTVShowQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<TVShowDto>> Handle(GetTVShowQuery request, CancellationToken cancellationToken)
        {
            var targetTVShow = await applicationDbContext
                .TVShows
                .AsNoTracking()
                .Where(x => x.Id == request.tVShowId)
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsSplitQuery()
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

            return new ApiResponseDto<TVShowDto>
            {
                Result = result,
                IsSucceed = true
            };
        }
    }
}
