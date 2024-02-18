using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Application.Services.Checkers;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Handlers
{
    public class AddNewMovieCommandHandler(ILogger<AddNewMovieCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentMovieOptions> contentOptions,
        IFileManager fileManager) 
        : IRequestHandler<AddNewMovieCommand, ApiResponseDto<MovieDto>>
    {
        private readonly ILogger<AddNewMovieCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentMovieOptions> contentOptions = contentOptions;
        private readonly IFileManager fileManager = fileManager;

        public async Task<ApiResponseDto<MovieDto>> Handle(AddNewMovieCommand request, CancellationToken cancellationToken)
        {
            if (!File.Exists(request.movieToInsertDto.Location))
            {
                logger.LogError($"The file location {request.movieToInsertDto.Location} does not exists");

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"The movie with file {request.movieToInsertDto.Location} does not exist"
                };
            }

            if (!MediaFileExtensionChecker.IsValidFileExtension(Path.GetExtension(request.movieToInsertDto.Location)))
            {
                logger.LogError($"The file extension {Path.GetExtension(request.movieToInsertDto.Location)} is not valid");

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"Invalid file extension {Path.GetExtension(request.movieToInsertDto.Location)}"
                };
            }

            var IsMovieExists = await applicationDbContext
                  .Movies
                  .AsNoTracking()
                  .AnyAsync(x => x.Title == request.movieToInsertDto.Title);

            if(IsMovieExists)
            {
                return new ApiResponseDto<MovieDto> 
                { 
                    Result = null!,
                    IsSucceed = false,
                    Message = $"The movie with title {request.movieToInsertDto.Title} is already exist"
                };
            }

            var movie = request.movieToInsertDto.Adapt<Movie>();

            if (movie is null)
            {
                logger.LogError($"The mapper package can not map the the {nameof(request.movieToInsertDto)} entity to {nameof(movie)}");

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"The mapper package can not map the the {nameof(request.movieToInsertDto)} entity to {nameof(movie)}"
                };
            }

            string movieFileNameToSave = $"{request.movieToInsertDto.Title}-{request.movieToInsertDto.ReleaseYear}-{Guid.NewGuid().ToString()[..4]}";

            fileManager.SaveTheOriginalAndCompressedContentFile(
                request.movieToInsertDto.Location,
                contentOptions.Value.TargetDirectoryToCompressTo,
                movieFileNameToSave);

            movie.Location = Convert.ToBase64String(Encoding.UTF8.GetBytes(movieFileNameToSave + $"{Path.GetExtension(request.movieToInsertDto.Location)}"));

            // save to the database
            try
            {
                var contentTags = await applicationDbContext
                .Tags
                .ToListAsync() ?? [];

                //add the new tags if exists
                foreach (var tag in request.movieToInsertDto.Tags)
                {
                    if (!contentTags.Any(x => x.TagValue.Equals(tag.TagValue, StringComparison.OrdinalIgnoreCase)))
                    {
                        contentTags.Add(new Tag { TagValue = tag.TagValue.ToLower() });
                    }
                }
                await applicationDbContext.SaveChangesAsync();

                movie.Tags.Clear();
                var tagsDictionary = contentTags.ToDictionary(k => k.TagValue.ToLower(), v => v);
                foreach (var tag in request.movieToInsertDto.Tags)
                {
                    movie.Tags.Add(tagsDictionary[tag.TagValue.ToLower()]);
                }

                logger.LogTrace($"Try to save the movie info in the database");

                await applicationDbContext.Movies.AddAsync(movie);

                await applicationDbContext.SaveChangesAsync();

                logger.LogTrace($"The movie added to the database successfully");

                return new ApiResponseDto<MovieDto>
                {
                    Result = movie.Adapt<MovieDto>(),
                    IsSucceed = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                // remove the added files 
                File.Delete(Path.Combine(contentOptions.Value.TargetDirectoryToSaveTo, movieFileNameToSave +
                    $"{Path.GetExtension(request.movieToInsertDto.Location)}"));
                File.Delete(Path.Combine(contentOptions.Value.TargetDirectoryToCompressTo, movieFileNameToSave) + ".gz");

                logger.LogError($"An error occur while saving the movie due to : {ex.Message}");

                return new ApiResponseDto<MovieDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = "An error occur while saving the entity"
                };
            }
        }
    }
}
