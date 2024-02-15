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
    public class GetTVShowEpisodeQueryHandler : IRequestHandler<GetTVShowEpisodeQuery, ApiResponseDto>
    {
        private readonly ILogger<GetTVShowEpisodeQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public GetTVShowEpisodeQueryHandler(ILogger<GetTVShowEpisodeQueryHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<ApiResponseDto> Handle(GetTVShowEpisodeQuery request, CancellationToken cancellationToken)
        {
            var targetTVShow = await applicationDbContext
                .TVShows
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsNoTracking()
                .SingleAsync(x => x.Id == request.tVShowEpisodeRequestDto.TVShowId);
            
            if(targetTVShow is null)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not find the target episode !"
                };
            }

            var targetSeason = targetTVShow
                .Seasons
                .First(x => x.Id == request.tVShowEpisodeRequestDto.TVShowSeasonId);

            var targetEpisode = targetSeason
                .Episodes
                .First(x => x.Id == request.tVShowEpisodeRequestDto.EpisodeId);

            targetEpisode.FileName = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                Encoding.UTF8.GetString(Convert.FromBase64String(targetTVShow.Location)),
                Encoding.UTF8.GetString(Convert.FromBase64String(targetSeason.DirectoryName)),
                Encoding.UTF8.GetString(Convert.FromBase64String(targetEpisode.FileName)));

            var test = File.Exists(targetEpisode.FileName);

            return new ApiResponseDto
            {
                Result = targetEpisode.Adapt<TVShowEpisodeDto>()
            };
        }
    }
}
