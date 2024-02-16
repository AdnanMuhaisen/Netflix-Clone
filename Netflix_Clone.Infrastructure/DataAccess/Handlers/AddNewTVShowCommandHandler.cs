using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class AddNewTVShowCommandHandler : IRequestHandler<AddNewTVShowCommand, ApiResponseDto>
    {
        private readonly ILogger<AddNewTVShowCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public AddNewTVShowCommandHandler(ILogger<AddNewTVShowCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }


        public async Task<ApiResponseDto> Handle(AddNewTVShowCommand request, CancellationToken cancellationToken)
        {
            var IsTheTargetShowExist = applicationDbContext
                .TVShows
                .Where(x => x.Title == request.tVShowToInsertDto.Title)
                .Any();

            if(IsTheTargetShowExist)
            {
                throw new InsertionException($"The TV Show with title : {request.tVShowToInsertDto.Title} is already exist");
            }

            var tvShowToInsert = request.tVShowToInsertDto.Adapt<TVShow>();

            //create an empty folder to save the tv show seasons and episodes:
            string pathOfDirectoryToInsert = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                    request.tVShowToInsertDto.Title);

            //represents the directory name
            tvShowToInsert.Location = pathOfDirectoryToInsert.Split('\\').Last();
            tvShowToInsert.Location = Convert.ToBase64String(Encoding.UTF8.GetBytes(tvShowToInsert.Location));

            try
            {
                var contentTags = await applicationDbContext
               .Tags
               .ToListAsync() ?? new List<Tag>();

                //add the new tags if exists
                foreach (var tag in request.tVShowToInsertDto.Tags)
                {
                    if (!contentTags.Any(x => x.TagValue.Equals(tag.TagValue, StringComparison.OrdinalIgnoreCase)))
                    {
                        contentTags.Add(new Tag { TagValue = tag.TagValue.ToLower() });
                    }
                }
                await applicationDbContext.SaveChangesAsync();

                tvShowToInsert.Tags.Clear();
                var tagsDictionary = contentTags.ToDictionary(k => k.TagValue.ToLower(), v => v);
                foreach (var tag in request.tVShowToInsertDto.Tags)
                {
                    tvShowToInsert.Tags.Add(tagsDictionary[tag.TagValue.ToLower()]);
                }

                var info = Directory.CreateDirectory(pathOfDirectoryToInsert);

                applicationDbContext.TVShows.Add(tvShowToInsert);
                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto { Result = tvShowToInsert.Adapt<TVShowDto>() };
            }
            catch (Exception ex)
            {
                Directory.Delete(pathOfDirectoryToInsert);

                throw new InsertionException($"Can not add the TV-Show with title : {request.tVShowToInsertDto.Title} because this exception : {ex.Message}");
            }
        }
    }
}
