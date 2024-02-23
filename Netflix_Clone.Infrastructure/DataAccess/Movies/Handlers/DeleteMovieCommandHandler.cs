using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;


namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class DeleteMovieCommandHandler(ILogger<DeleteMovieCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentMovieOptions> options,
        IFileManager fileManager) 
        : IRequestHandler<DeleteMovieCommand, ApiResponseDto<bool>>
    {
        private readonly ILogger<DeleteMovieCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentMovieOptions> options = options;
        private readonly IFileManager fileManager = fileManager;

        public async Task<ApiResponseDto<bool>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The delete movie handler is start to execute");

            var targetMovieToDelete = await applicationDbContext
                .Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.contentId);

            logger.LogTrace("Try to find the movie with id = {id}", request.contentId);

            if(targetMovieToDelete is null)
            {
                logger.LogError("The movie with the ID = {id} was not found", request.contentId);

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    IsSucceed = false,
                    Message = $"The movie with the ID = {request.contentId} was not found"
                };
            }

            // delete the movie content from the server:
            //delete the video itself:
            string encodedFilePathOfTheTargetMovie = Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToDelete.Location));
            string targetFileName = encodedFilePathOfTheTargetMovie[..encodedFilePathOfTheTargetMovie.IndexOf('.')];

            bool IsOriginalFileDeleted = fileManager
                .FindAndDeleteAFile(options.Value.TargetDirectoryToSaveTo, targetFileName);

            if(!IsOriginalFileDeleted)
            {
                logger.LogInformation($"Can not delete the file with name : {targetFileName}");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    IsSucceed = false,
                    Message = "Can not delete the file"
                };
            }

            //delete the compressed file
            bool IsCompressedFileDeleted = fileManager
                .FindAndDeleteAFile(options.Value.TargetDirectoryToCompressTo, targetFileName);

            if(!IsCompressedFileDeleted)
            {
                logger.LogInformation($"Can not delete the compressed file");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    IsSucceed = false,
                    Message = "Can not delete the compressed file"
                };
            }

            //delete from the database
            try
            {
                logger.LogTrace("Try to delete the movie with id = {id} from the database", targetMovieToDelete.Id);

                applicationDbContext.Movies.Remove(targetMovieToDelete);

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace("The movie with id = {id} is deleted from the database", targetMovieToDelete.Id);

                return new ApiResponseDto<bool>
                {
                    Result = true,
                    IsSucceed = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                logger.LogError("The movie with id = {id} is failed to delete from the database due to this exception: {exMessage}", targetMovieToDelete.Id, ex.Message);

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    IsSucceed = false,
                    Message = "Can not delete the compressed file"
                };
            }
        }
    }
}
