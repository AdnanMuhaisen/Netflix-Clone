using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DownloadMovieCommandHandler : IRequestHandler<DownloadMovieCommand, ApiResponseDto>
    {
        private readonly ILogger<DownloadMovieCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IFileManager fileManager;
        private readonly IOptions<ContentMovieOptions> options;

        public DownloadMovieCommandHandler(ILogger<DownloadMovieCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IFileManager fileManager,
            IOptions<ContentMovieOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.fileManager = fileManager;
            this.options = options;
        }


        public async Task<ApiResponseDto> Handle(DownloadMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The download movie command handler is started to execute");

            request.downloadMovieRequestDto.PathToDownloadFor =
                (string.IsNullOrEmpty(request.downloadMovieRequestDto.PathToDownloadFor))
                ? options.Value.DefaultPathToDownload
                : request.downloadMovieRequestDto.PathToDownloadFor;

            if (!Directory.Exists(request.downloadMovieRequestDto.PathToDownloadFor))
            {
                logger.LogError("Can not download the movie to a non existing location : {location}!", request.downloadMovieRequestDto.PathToDownloadFor);

                return new ApiResponseDto
                {
                    Result = new DownloadMovieResponseDto { IsDownloaded = false },
                    Message = $"Can not download the movie in this location : {request.downloadMovieRequestDto.PathToDownloadFor}"
                };
            }

            logger.LogTrace("Try to get the target movie with id {id}", request.downloadMovieRequestDto.MovieId);

            var targetMovie = await applicationDbContext
                .Movies
                .FindAsync(request.downloadMovieRequestDto.MovieId);

            if(targetMovie is null)
            {
                logger.LogError("The requested movie with id {id} does not exist !", request.downloadMovieRequestDto.PathToDownloadFor);

                throw new EntityNotFoundException($"Can not find the target movie with id {request.downloadMovieRequestDto.MovieId}");
            }

            if (!targetMovie.IsAvailableToDownload)
            {
                logger.LogError("Can not download move that is unavailable to download");

                throw new ContentDownloadException("The movie is unavailable to download");
            }

            //validate if the user verified to download based on user subscription plan
            var activeUserSubscriptionPlan = await applicationDbContext
                .UsersSubscriptions
                .AsNoTracking()
                .Include(x => x.SubscriptionPlan)
                .ThenInclude(x => x.PlanFeatures)
                .Where(x => x.UserId == request.userId && DateTime.UtcNow <= x.EndDate)
                .FirstOrDefaultAsync();

            if(activeUserSubscriptionPlan is null)
            {
                throw new ContentDownloadException($"A user with an ID {request.userId} does not have an active subscription !");
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
                .Where(x => Char.IsDigit(x))
                .First()
                .ToString()
                );

            if (numberOfUserDownloadsForTheTargetSubscription >= downloadTimesSupportedByTheSubscriptionPlan)
            {
                throw new ContentDownloadException($"The user can not download the movie because it is exceeds the times of " +
                    $"downloads supported by the subscription !");
            }

            logger.LogTrace("The target movie with id : {id} is retrieved", targetMovie.Id);

            string encodedLocationForTheTargetMovie = Encoding.UTF8.GetString(Convert.FromBase64String(targetMovie.Location));
            string targetMovieFilePath = Path.Combine(options.Value.TargetDirectoryToSaveTo, encodedLocationForTheTargetMovie);

            try
            {
                //update the movie info in the database 
                logger.LogTrace("Try to update the movie info in the database");

                targetMovie.TotalNumberOfDownloads++;

                applicationDbContext.Update(targetMovie);

                string nameOfTheDownloadedFile = $"Copy-{encodedLocationForTheTargetMovie.Substring(0, encodedLocationForTheTargetMovie.IndexOf('.'))}" +
                    $"-{Guid.NewGuid().ToString().Substring(0, 4)}" +
                    $"{Path.GetExtension(encodedLocationForTheTargetMovie)}";

                File.Copy(targetMovieFilePath, Path.Combine(request.downloadMovieRequestDto.PathToDownloadFor, nameOfTheDownloadedFile), true);

                logger.LogTrace("The movie is downloaded successfully to the path : {path}", request.downloadMovieRequestDto.PathToDownloadFor);

                //add to user downloads
                await applicationDbContext
                    .UsersDownloads
                    .AddAsync(new ContentDownload
                    {
                        ApplicationUserId = request.userId,
                        ContentId = request.downloadMovieRequestDto.MovieId,
                        DownloadedAt = DateTime.UtcNow
                    });

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace("The movie updated successfully in the database");

                return new ApiResponseDto
                {
                    Result = new DownloadMovieResponseDto { IsDownloaded = true },
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while trying to download the movie with id {id}", targetMovie.Id);

                return new ApiResponseDto
                {
                    Result = new DownloadMovieResponseDto { IsDownloaded = false },
                    Message = ex.Message
                };
            }
        }
    }
}
