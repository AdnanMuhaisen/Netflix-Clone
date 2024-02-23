using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Queries;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Handlers
{
    public class GetTVShowEpisodeQueryHandler(ILogger<GetTVShowEpisodeQueryHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<GetTVShowEpisodeQuery, ApiResponseDto<TVShowEpisodeDto>>
    {
        private readonly ILogger<GetTVShowEpisodeQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<TVShowEpisodeDto>> Handle(GetTVShowEpisodeQuery request, CancellationToken cancellationToken)
        {
            var targetTVShow = await applicationDbContext
                .TVShows
                .AsNoTracking()
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsSplitQuery()
                .SingleAsync(x => x.Id == request.tVShowEpisodeRequestDto.TVShowId);

            if (targetTVShow is null)
            {
                logger.LogInformation($"Can not find the tv show with id : {request.tVShowEpisodeRequestDto.TVShowId}" +
                    $" to get the episode with id : {request.tVShowEpisodeRequestDto.EpisodeId}");

                return new ApiResponseDto<TVShowEpisodeDto>
                {
                    Result = null!,
                    Message = "Can not find the target episode !",
                    IsSucceed = false
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

            logger.LogInformation($"The episode with id : {request.tVShowEpisodeRequestDto.EpisodeId} is " +
                $"retrieved successfully");

            return new ApiResponseDto<TVShowEpisodeDto>
            {
                Result = targetEpisode.Adapt<TVShowEpisodeDto>(),
                IsSucceed = true
            };
        }
    }
}
