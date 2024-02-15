using MediatR;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Domain.Exceptions;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain;
using System.Text;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.DTOs;


namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, ApiResponseDto>
    {
        private readonly ILogger<DeleteMovieCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentMovieOptions> options;
        private readonly IFileManager fileManager;

        public DeleteMovieCommandHandler(ILogger<DeleteMovieCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentMovieOptions> options,
            IFileManager fileManager)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
            this.fileManager = fileManager;
        }

        public async Task<ApiResponseDto> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The delete movie handler is start to execute");

            var targetMovieToDelete = await applicationDbContext
                .Movies
                .FindAsync(request.contentId);

            logger.LogTrace("Try to find the movie with id = {id}", request.contentId);

            if(targetMovieToDelete is null)
            {
                logger.LogError("The movie with the ID = {id} was not found", request.contentId);

                throw new EntityNotFoundException($"Movie with the id {request.contentId} was not found");
            }

            // delete the movie content from the server:
            //delete the video itself:
            string encodedFilePathOfTheTargetMovie = Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToDelete.Location));
            string targetFileName = encodedFilePathOfTheTargetMovie.Substring(0, encodedFilePathOfTheTargetMovie.IndexOf('.'));

            bool IsOriginalFileDeleted = fileManager.FindAndDeleteAFile(options.Value.TargetDirectoryToSaveTo, targetFileName);

            if(!IsOriginalFileDeleted)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not delete the file"
                };
            }

            //delete the compressed file
            bool IsCompressedFileDeleted = fileManager.FindAndDeleteAFile(options.Value.TargetDirectoryToCompressTo, targetFileName);

            if(!IsCompressedFileDeleted)
            {
                return new ApiResponseDto
                {
                    Result = null!,
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

                return new ApiResponseDto
                {
                    Result = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError("The movie with id = {id} is failed to delete from the database due to this exception: {exMessage}", targetMovieToDelete.Id, ex.Message);

                return new ApiResponseDto
                {
                    Result = null!,
                    Message = ex.Message
                };
            }
        }
    }
}
