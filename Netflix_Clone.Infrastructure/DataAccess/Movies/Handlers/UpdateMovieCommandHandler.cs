using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class UpdateMovieCommandHandler(ApplicationDbContext applicationDbContext,
        ILogger<UpdateMovieCommandHandler> logger,
        IFileManager fileManager,
        IOptions<ContentMovieOptions> options) : IRequestHandler<UpdateMovieCommand, ApiResponseDto<MovieDto>>
    {
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly ILogger<UpdateMovieCommandHandler> logger = logger;
        private readonly IFileManager fileManager = fileManager;
        private readonly IOptions<ContentMovieOptions> options = options;

        public async Task<ApiResponseDto<MovieDto>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The update movie handler is start to execute");

            var targetMovieToUpdate = await applicationDbContext
                .Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.movieDto.Id);

            if (targetMovieToUpdate is null)
            {
                logger.LogError("The target movie to update with id {id} does not exist", request.movieDto.Id);

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    Message = $"can not find the movie {nameof(targetMovieToUpdate)}",
                    IsSucceed = false
                };
            }

            //now try to update:

            var updatedMovie = request.movieDto.Adapt<Movie>();

            var encodedContentFilePathOfTheTargetMovieToUpdate = Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToUpdate!.Location));

            if (encodedContentFilePathOfTheTargetMovieToUpdate != request.movieDto.Location.Substring(request.movieDto.Location.LastIndexOf('\\')).Trim('\\'))
            {
                // we need to remove the existing files 
                // and then add the new files 

                logger.LogTrace("Try to delete existing files for the movie with id {id}", targetMovieToUpdate.Id);

                //delete the file
                string currentFileName = encodedContentFilePathOfTheTargetMovieToUpdate.Substring(0, encodedContentFilePathOfTheTargetMovieToUpdate.IndexOf('.'));
                bool deleteOriginalFileResult = fileManager.FindAndDeleteAFile(options.Value.TargetDirectoryToSaveTo, currentFileName);

                if (!deleteOriginalFileResult)
                {
                    logger.LogError("An error occur when try to delete the original file of the movie with id : {id}", request.movieDto.Id);

                    return new ApiResponseDto<MovieDto>
                    {
                        Result = null!,
                        Message = $"Can not delete the original file that contains this segment {currentFileName}",
                        IsSucceed = false
                    };
                }

                logger.LogTrace("The original file of the movie with id : {id} deleted successfully", request.movieDto.Id);

                bool deleteCompressedFileResult = fileManager.FindAndDeleteAFile(options.Value.TargetDirectoryToCompressTo, currentFileName);

                if (!deleteCompressedFileResult)
                {
                    logger.LogError("An error occur when try to delete the compressed file of the movie with id : {id}", request.movieDto.Id);

                    return new ApiResponseDto<MovieDto>
                    {
                        Result = null!,
                        Message = $"Can not delete the compressed file that contains this segment {currentFileName}",
                        IsSucceed = false
                    };
                }

                logger.LogTrace("The compressed file of the movie with id : {id} deleted successfully", request.movieDto.Id);

                string movieFileNameToSave = $"{request.movieDto.Title}-{request.movieDto.ReleaseYear}-{Guid.NewGuid().ToString().Substring(0, 4)}";

                // now add the new files:
                fileManager.SaveTheOriginalAndCompressedContentFile(
                    request.movieDto.Location,
                    options.Value.TargetDirectoryToCompressTo,
                    movieFileNameToSave
                    );

                logger.LogTrace("The new compressed file of the movie with id : {id} is saved successfully", request.movieDto.Id);

                updatedMovie.Location = Convert.ToBase64String(Encoding.UTF8.GetBytes(movieFileNameToSave + $"{Path.GetExtension(request.movieDto.Location)}"));
            }

            //update in the database
            try
            {
                applicationDbContext.Update(updatedMovie);

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace("The movie with id {id} is updated successfully", request.movieDto.Id);

                return new ApiResponseDto<MovieDto>
                {
                    Result = request.movieDto,
                    Message = string.Empty,
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                logger.LogTrace("The movie with id {id} is failed to update because this exception : {message}", request.movieDto.Id, ex.Message);

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    Message = $"can not update the movie content",
                    IsSucceed = false
                };
            }
        }
    }
}
