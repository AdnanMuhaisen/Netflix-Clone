using Mapster;
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
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, MovieDto>
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<UpdateMovieCommandHandler> logger;
        private readonly IFileManager fileManager;
        private readonly IOptions<ContentMovieOptions> options;

        public UpdateMovieCommandHandler(ApplicationDbContext applicationDbContext,
            ILogger<UpdateMovieCommandHandler> logger,
            IFileManager fileManager,
            IOptions<ContentMovieOptions> options)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
            this.fileManager = fileManager;
            this.options = options;
        }

        public async Task<MovieDto> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The update movie handler is start to execute");

            var targetMovieToUpdate = applicationDbContext
                .Movies
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == request.movieDto.Id);

            if(targetMovieToUpdate is null)
            {
                logger.LogError("The target movie to update with id {id} does not exist", request.movieDto.Id);

                ArgumentNullException.ThrowIfNull($"can not find the movie {nameof(targetMovieToUpdate)}");
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

                    throw new InvalidUpdateOperationException($"Can not delete the original file that contains this segment {currentFileName}");
                }

                logger.LogTrace("The original file of the movie with id : {id} deleted successfully", request.movieDto.Id);

                bool deleteCompressedFileResult = fileManager.FindAndDeleteAFile(options.Value.TargetDirectoryToCompressTo, currentFileName);

                if (!deleteCompressedFileResult)
                {
                    logger.LogError("An error occur when try to delete the compressed file of the movie with id : {id}", request.movieDto.Id);

                    throw new InvalidUpdateOperationException($"Can not delete the compressed file that contains this segment {currentFileName}");
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
                applicationDbContext.Movies.Update(updatedMovie);

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace("The movie with id {id} is updated successfully", request.movieDto.Id);

                return request.movieDto;
            }
            catch ( Exception ex )
            {
                logger.LogTrace("The movie with id {id} is failed to update because this exception : {message}", request.movieDto.Id, ex.Message);

                throw new InvalidUpdateOperationException($"can not update the movie content because this exception {ex.Message}");
            }
        }
    }
}
