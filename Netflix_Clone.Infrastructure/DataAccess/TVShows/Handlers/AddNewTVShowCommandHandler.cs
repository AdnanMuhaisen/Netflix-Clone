using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Handlers
{
    public class AddNewTVShowCommandHandler(ILogger<AddNewTVShowCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<AddNewTVShowCommand, ApiResponseDto<TVShowDto>>
    {
        private readonly ILogger<AddNewTVShowCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<TVShowDto>> Handle(AddNewTVShowCommand request, CancellationToken cancellationToken)
        {
            var IsTheTargetShowExist = applicationDbContext
                .TVShows
                .AsNoTracking()
                .Where(x => x.Title == request.tVShowToInsertDto.Title)
                .Any();

            if(IsTheTargetShowExist)
            {
                return new ApiResponseDto<TVShowDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"The TV Show with title : {request.tVShowToInsertDto.Title} is already exist"
                };
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
               .ToListAsync() ?? [];

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

                return new ApiResponseDto<TVShowDto>
                {
                    Result = tvShowToInsert.Adapt<TVShowDto>(),
                    IsSucceed = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                Directory.Delete(pathOfDirectoryToInsert);

                return new ApiResponseDto<TVShowDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = $"Can not add the TV-Show with title : {request.tVShowToInsertDto.Title} because this exception : {ex.Message}"
                };
            }
        }
    }
}
