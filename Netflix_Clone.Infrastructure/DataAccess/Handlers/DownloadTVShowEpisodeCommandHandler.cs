using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DownloadTVShowEpisodeCommandHandler : IRequestHandler<DownloadTVShowEpisodeCommand, ApiResponseDto>
    {
        private readonly ILogger<DownloadTVShowEpisodeCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public DownloadTVShowEpisodeCommandHandler(ILogger<DownloadTVShowEpisodeCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<ApiResponseDto> Handle(DownloadTVShowEpisodeCommand request, CancellationToken cancellationToken)
        {
            var targetTVShow = applicationDbContext
                .TVShows
                .AsNoTracking()
                .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
                .FirstOrDefault(x => x.Id == request.downloadEpisodeRequestDto.TVShowId);
                
            if(targetTVShow is null)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not find the target TV Show"
                };
            }

            if(!targetTVShow.IsAvailableToDownload)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"The {targetTVShow.Title} TvShow Is unavailable to download"
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

            if(!File.Exists(targetEpisodeFilePath))
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"The episode file can not be found"
                };
            }

            request.downloadEpisodeRequestDto.PathToDownloadFor =
                (string.IsNullOrEmpty(request.downloadEpisodeRequestDto.PathToDownloadFor))
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

                return new ApiResponseDto
                {
                    Result = nameOfTheFileToDownload,
                    Message = "Downloaded successfully"
                };
            }
            catch(Exception ex)
            {
                if (File.Exists(Path.Combine(request.downloadEpisodeRequestDto.PathToDownloadFor,
                    nameOfTheFileToDownload)))
                {
                    File.Delete(Path.Combine(request.downloadEpisodeRequestDto.PathToDownloadFor,
                        nameOfTheFileToDownload));
                }

                //log

                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not download the episode"
                };
            }
        }
    }
}
