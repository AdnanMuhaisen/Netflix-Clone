using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Handlers
{
    public class AddNewTVShowSeasonCommandHandler(ILogger<AddNewTVShowSeasonCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<AddNewTVShowSeasonCommand, ApiResponseDto<TVShowSeasonDto>>
    {
        private readonly ILogger<AddNewTVShowSeasonCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<TVShowSeasonDto>> Handle(AddNewTVShowSeasonCommand request, CancellationToken cancellationToken)
        {
            var currentTVShowSeasons = await applicationDbContext
                .TVShowsSeasons
                .AsNoTracking()
                .Where(x => x.TVShowId == request.tVShowSeasonToInsertDto.TVShowId)
                .ToListAsync();

            if(currentTVShowSeasons.Any(x=>x.SeasonNumber == request.tVShowSeasonToInsertDto.SeasonNumber 
                || x.SeasonName == request.tVShowSeasonToInsertDto.SeasonName))
            {
                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    Message = $"Can not add the season number: {request.tVShowSeasonToInsertDto.SeasonNumber}" +
                    $"with name : {request.tVShowSeasonToInsertDto.SeasonName} because it is already exist",
                    IsSucceed = false
                };
            }

            //the name of the TV SHow directory is the title of the tv show itself
            var targetSeasonTVShow = (await applicationDbContext
                .TVShows
                .SingleOrDefaultAsync(x => x.Id == request.tVShowSeasonToInsertDto.TVShowId));

            if(targetSeasonTVShow is null)
            {
                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    IsSucceed = false,
                    Message = "Can not find the target season"
                };
            }

            string targetTVShowDirectoryName = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                targetSeasonTVShow.Title);

            if(!Directory.Exists(targetTVShowDirectoryName))
            {
                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    Message = $"The target TV Show with id : {request.tVShowSeasonToInsertDto.TVShowId}" +
                    $" that you want to add season for does not exist",
                    IsSucceed = false
                };
            }

            var lastSeasonNumberForTheTargetTVShow = Directory
                .GetDirectories(targetTVShowDirectoryName)
                .Length;

            if(request.tVShowSeasonToInsertDto.SeasonNumber != (lastSeasonNumberForTheTargetTVShow + 1))
            {
                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    Message = $"Can not add season with number: {request.tVShowSeasonToInsertDto.SeasonNumber}" +
                    $"because the last season number is equals : {lastSeasonNumberForTheTargetTVShow}",
                    IsSucceed = false
                };
            }

            //the format of the season folder is that : {TVShow Title}-{SeasonNumber}-{SeasonName(if exists)}            
            var targetSeasonDirectoryName = new StringBuilder($"{targetSeasonTVShow.Title}-{request.tVShowSeasonToInsertDto.SeasonNumber}");
            if (request.tVShowSeasonToInsertDto.SeasonName is not null)
                targetSeasonDirectoryName.Append($"-{request.tVShowSeasonToInsertDto.SeasonName}");

            //add season for the TV Show:
            //add the season folder :
            string pathOfTheDirectoryToAdd = Path.Combine(targetTVShowDirectoryName,
                targetSeasonDirectoryName.ToString());

            try
            {
                Directory.CreateDirectory(pathOfTheDirectoryToAdd);
            }
            catch (Exception ex) 
            {
                //log
                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    Message = $"{ex.Message}",
                    IsSucceed = false
                };
            }

            //save to the database
            var seasonToAdd = request.tVShowSeasonToInsertDto.Adapt<TVShowSeason>();

            seasonToAdd.DirectoryName = Convert.ToBase64String(Encoding.UTF8.GetBytes(targetSeasonDirectoryName.ToString()));

            ArgumentNullException.ThrowIfNull(seasonToAdd);

            try
            {
                await applicationDbContext
                    .TVShowsSeasons
                    .AddAsync(seasonToAdd);

                targetSeasonTVShow.TotalNumberOfSeasons++;

                await applicationDbContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            { 
                Directory.Delete(pathOfTheDirectoryToAdd, true);

                return new ApiResponseDto<TVShowSeasonDto>
                {
                    Result = null!,
                    Message = $"Can not add season with number: {request.tVShowSeasonToInsertDto.SeasonNumber}",
                    IsSucceed = false
                };
            }

            return new ApiResponseDto<TVShowSeasonDto>
            { 
                Result = seasonToAdd.Adapt<TVShowSeasonDto>() ,
                IsSucceed = true
            };
        }
    }
}
