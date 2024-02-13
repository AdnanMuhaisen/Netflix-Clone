using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DownloadMovieCommandHandler : IRequestHandler<DownloadMovieCommand, DownloadMovieResponseDto>
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


        public async Task<DownloadMovieResponseDto> Handle(DownloadMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The download movie command handler is started to execute");

            if(!Directory.Exists(request.downloadMovieRequestDto.PathToDownloadFor))
            {
                logger.LogError("Can not download the movie to a non existing location : {location}!", request.downloadMovieRequestDto.PathToDownloadFor);

                return new DownloadMovieResponseDto
                {
                    IsDownloaded = false,
                    Message = $"Can not download the movie in this location : {request.downloadMovieRequestDto.PathToDownloadFor}"
                };
            }
            // check if the user verified to download the movie
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

                throw new MovieDownloadException("The movie is unavailable to download");
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
                    $"{Path.GetExtension(encodedLocationForTheTargetMovie)}";

                File.Copy(targetMovieFilePath, Path.Combine(request.downloadMovieRequestDto.PathToDownloadFor, nameOfTheDownloadedFile), true);

                logger.LogTrace("The movie is downloaded successfully to the path : {path}", request.downloadMovieRequestDto.PathToDownloadFor);

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace("The movie updated successfully in the database");

                return new DownloadMovieResponseDto { IsDownloaded = true, Message = string.Empty };
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while trying to download the movie with id {id}", targetMovie.Id);

                return new DownloadMovieResponseDto { IsDownloaded = false, Message = ex.Message };
            }
        }
    }
}
