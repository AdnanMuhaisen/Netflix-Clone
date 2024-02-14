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
    public class AddNewTVShowSeasonCommandHandler : IRequestHandler<AddNewTVShowSeasonCommand, TVShowSeasonDto>
    {
        private readonly ILogger<AddNewTVShowSeasonCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public AddNewTVShowSeasonCommandHandler(ILogger<AddNewTVShowSeasonCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }
        public async Task<TVShowSeasonDto> Handle(AddNewTVShowSeasonCommand request, CancellationToken cancellationToken)
        {
            var currentTVShowSeasons = await applicationDbContext
                .TVShowsSeasons
                .Where(x => x.TVShowId == request.tVShowSeasonToInsertDto.TVShowId)
                .ToListAsync();

            if(currentTVShowSeasons.Any(x=>x.SeasonNumber == request.tVShowSeasonToInsertDto.SeasonNumber 
                || x.SeasonName == request.tVShowSeasonToInsertDto.SeasonName))
            {
                throw new InsertionException($"Can not add the season number: {request.tVShowSeasonToInsertDto.SeasonNumber}" +
                    $"with name : {request.tVShowSeasonToInsertDto.SeasonName} because it is already exist");
            }

            //the name of the TV SHow directory is the title of the tv show itself
            var targetSeasonTVShow = (await applicationDbContext
                .TVShows
                .SingleOrDefaultAsync(x => x.Id == request.tVShowSeasonToInsertDto.TVShowId));

            ArgumentNullException.ThrowIfNull(targetSeasonTVShow);

            string targetTVShowDirectoryName = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                targetSeasonTVShow.Title);

            if(!Directory.Exists(targetTVShowDirectoryName))
            {
                throw new InvalidDirectoryCreationException($"The target TV Show with id : {request.tVShowSeasonToInsertDto.TVShowId}" +
                    $" that you want to add season for does not exist");
            }

            var lastSeasonNumberForTheTargetTVShow = Directory
                .GetDirectories(targetTVShowDirectoryName)
                .Count();

            if(request.tVShowSeasonToInsertDto.SeasonNumber != (lastSeasonNumberForTheTargetTVShow + 1))
            {
                throw new InvalidDirectoryCreationException($"Can not add season with number: {request.tVShowSeasonToInsertDto.SeasonNumber}" +
                    $"because the last season number is equals : {lastSeasonNumberForTheTargetTVShow}");
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
                throw new InvalidDirectoryCreationException(ex.Message);
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

                throw new InsertionException(ex.Message);
            }

            return seasonToAdd.Adapt<TVShowSeasonDto>();
        }
    }
}
