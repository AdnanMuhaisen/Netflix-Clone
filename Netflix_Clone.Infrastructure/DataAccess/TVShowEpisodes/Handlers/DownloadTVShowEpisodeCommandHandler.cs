using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Handlers
{
    public class DownloadTVShowEpisodeCommandHandler(ILogger<DownloadTVShowEpisodeCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) 
        : IRequestHandler<DownloadTVShowEpisodeCommand, ApiResponseDto<string>>
    {
        private readonly ILogger<DownloadTVShowEpisodeCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<string>> Handle(DownloadTVShowEpisodeCommand request, CancellationToken cancellationToken)
        {
            var targetTVShow = applicationDbContext
                .TVShows
                .AsNoTracking()
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .AsSplitQuery()
                .FirstOrDefault(x => x.Id == request.downloadEpisodeRequestDto.TVShowId);

            if (targetTVShow is null)
            {
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    Message = "Can not find the target TV Show",
                    IsSucceed = false
                };
            }

            if (!targetTVShow.IsAvailableToDownload)
            {
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    Message = $"The {targetTVShow.Title} TvShow Is unavailable to download",
                    IsSucceed = false
                };
            }

            //validate if the user verified to download based on user subscription plan
            var activeUserSubscriptionPlan = await applicationDbContext
                .UsersSubscriptions
                .AsNoTracking()
                .Include(x => x.SubscriptionPlan)
                .ThenInclude(x => x.PlanFeatures)
                .AsSplitQuery()
                .Where(x => x.UserId == request.userId && DateTime.UtcNow <= x.EndDate)
                .FirstOrDefaultAsync();
                
            if(activeUserSubscriptionPlan is null)
            {
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"A user with an ID {request.userId} does not have an active subscription !"
                };
            }

            //check based on the plan and the previous user downloads if the 
            //user can download more movies
            var numberOfUserDownloadsForTheTargetSubscription = applicationDbContext
                .UsersDownloads
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == request.userId && x.DownloadedAt >= activeUserSubscriptionPlan.StartDate)
                .Count();

            var downloadFeatureOfTheSubscriptionPlan = activeUserSubscriptionPlan
                .SubscriptionPlan
                .PlanFeatures
                .Where(x => x.Feature.StartsWith("Download", StringComparison.OrdinalIgnoreCase))
                .First();

            int downloadTimesSupportedByTheSubscriptionPlan = int.Parse(downloadFeatureOfTheSubscriptionPlan
                .Feature
                .Where(x => char.IsDigit(x))
                .First()
                .ToString()
                );

            if (numberOfUserDownloadsForTheTargetSubscription >= downloadTimesSupportedByTheSubscriptionPlan)
            {
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"The user can not download the movie because it is exceeds the times of " +
                    $"downloads supported by the subscription !"
                };
            }


            var targetSeason = targetTVShow
                .Seasons
                .First(x => x.Id == request.downloadEpisodeRequestDto.SeasonId);

            var targetEpisode = targetSeason
                .Episodes
                .First(x => x.Id == request.downloadEpisodeRequestDto.EpisodeId);

            string targetEpisodeFilePath = Path.Combine(
                options.Value.TargetDirectoryToSaveTo,
                Encoding.UTF8.GetString(Convert.FromBase64String(targetTVShow.Location)),
                Encoding.UTF8.GetString(Convert.FromBase64String(targetSeason.DirectoryName)),
                Encoding.UTF8.GetString(Convert.FromBase64String(targetEpisode.FileName)));

            if (!File.Exists(targetEpisodeFilePath))
            {
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    Message = $"The episode file can not be found",
                    IsSucceed = false
                };
            }

            request.downloadEpisodeRequestDto.PathToDownloadFor =
                string.IsNullOrEmpty(request.downloadEpisodeRequestDto.PathToDownloadFor)
                ? options.Value.DefaultPathToDownload
                : request.downloadEpisodeRequestDto.PathToDownloadFor;

            string nameOfTheFileToDownload = $"Copy" +
                $"-{Guid.NewGuid().ToString().Substring(0, 4)}" +
                $"-{Encoding.UTF8.GetString(Convert.FromBase64String(targetEpisode.FileName))}";

            //copy the file and update the Content info in the database
            try
            {
                File.Copy(targetEpisodeFilePath,
                    Path.Combine(request.downloadEpisodeRequestDto.PathToDownloadFor, nameOfTheFileToDownload),
                    true);

                targetTVShow.TotalNumberOfDownloads++;

                //add to user downloads table
                await applicationDbContext
                    .UsersDownloads
                    .AddAsync(new ContentDownload
                    {
                        ApplicationUserId = request.userId,
                        ContentId = request.downloadEpisodeRequestDto.TVShowId
                    });

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto<string>
                {
                    Result = nameOfTheFileToDownload,
                    Message = "Downloaded successfully",
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                if (File.Exists(Path.Combine(request.downloadEpisodeRequestDto.PathToDownloadFor,
                    nameOfTheFileToDownload)))
                {
                    File.Delete(Path.Combine(request.downloadEpisodeRequestDto.PathToDownloadFor,
                        nameOfTheFileToDownload));
                }

                //log
                return new ApiResponseDto<string>
                {
                    Result = null!,
                    Message = "Can not download the episode",
                    IsSucceed = false
                };
            }
        }
    }
}
