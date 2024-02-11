using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Serilog;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class AddNewMovieHandler : IRequestHandler<AddNewMovieCommand, MovieDto>
    {
        private readonly ILogger<AddNewMovieHandler> logger;
        private readonly IFileCompressor fileCompressor;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentOptions> contentOptions;

        public AddNewMovieHandler(ILogger<AddNewMovieHandler> logger,
            IFileCompressor fileCompressor,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentOptions> contentOptions)
        {
            this.logger = logger;
            this.fileCompressor = fileCompressor;
            this.applicationDbContext = applicationDbContext;
            this.contentOptions = contentOptions;
        }

        public async Task<MovieDto> Handle(AddNewMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = request.movieToInsertDto.Adapt<Movie>();

            if (movie is null)
            {
                logger.LogError($"The mapper package can not map the the {nameof(request.movieToInsertDto)} entity to {nameof(movie)}");

                throw new InvalidMappingOperationException($"The mapper package can not map the the {nameof(request.movieToInsertDto)} entity to {nameof(movie)}");
            }

            string movieFileNameToSave = $"{request.movieToInsertDto.Title}-{request.movieToInsertDto.ReleaseYear}-{Guid.NewGuid().ToString().Substring(0, 4)}";

            logger.LogTrace($"Try to compress and save the file {nameof(movieFileNameToSave)}");

            fileCompressor.CompressFileTo(
                request.movieToInsertDto.Location,
                contentOptions.Value.TargetDirectoryToCompressTo,
                movieFileNameToSave
                );

            File.Copy(sourceFileName: request.movieToInsertDto.Location,
                destFileName: Path.Combine(contentOptions.Value.TargetDirectoryToSaveTo,
                movieFileNameToSave) + "." + Path.GetExtension(request.movieToInsertDto.Location));

            string FileName = movieFileNameToSave + "." + Path.GetExtension(request.movieToInsertDto.Location);
            movie.Location = Convert.ToBase64String(Encoding.UTF8.GetBytes(FileName));

            logger.LogTrace($"The file {nameof(movieFileNameToSave)} compressed successfully");

            // save to the database
            try
            {
                logger.LogTrace($"Try to save the movie info in the database");

                await applicationDbContext.Movies.AddAsync(movie);
                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace($"The movie added to the database successfully");

                var result = movie.Adapt<MovieDto>();
                
                return result;
            }
            catch (Exception ex)
            {
                // remove the added files 
                File.Delete(Path.Combine(contentOptions.Value.TargetDirectoryToSaveTo, FileName));
                File.Delete(Path.Combine(contentOptions.Value.TargetDirectoryToCompressTo, movieFileNameToSave) + ".gz");

                logger.LogError($"An error occur while saving the movie due to : {ex.Message}");

                throw new InsertionException(ex.Message);
            }
        }

    }
}
