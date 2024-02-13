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
    public class AddNewTVShowEpisodeCommandHandler : IRequestHandler<AddNewTVShowEpisodeCommand, TVShowEpisodeDto>
    {
        private readonly ILogger<AddNewTVShowEpisodeCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public AddNewTVShowEpisodeCommandHandler(ILogger<AddNewTVShowEpisodeCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<TVShowEpisodeDto> Handle(AddNewTVShowEpisodeCommand request, CancellationToken cancellationToken)
        {
            var targetTVShow = await applicationDbContext
                .TVShows
                .FindAsync(request.TVShowEpisodeToInsertDto.TVShowId);

            if(targetTVShow is null)
            {
                throw new InsertionException($"The target TV Show with Id: {request.TVShowEpisodeToInsertDto.TVShowId} does not exist !");
            }

            var targetSeason = await applicationDbContext
                .TVShowsSeasons
                .FindAsync(request.TVShowEpisodeToInsertDto.SeasonId);

            if (targetSeason is null)
            {
                throw new InsertionException($"The target TV Show season with Id: {request.TVShowEpisodeToInsertDto.SeasonId} does not exist !");
            }

            var IsTargetEpisodeExist = await applicationDbContext
                .TVShowEpisodes
                .Where(x => x.TVShowId == request.TVShowEpisodeToInsertDto.TVShowId
                && x.SeasonId == request.TVShowEpisodeToInsertDto.SeasonId
                && x.EpisodeNumber == request.TVShowEpisodeToInsertDto.EpisodeNumber)
                .AnyAsync();

            if (IsTargetEpisodeExist)
            {
                throw new InsertionException($"The target Episode with number : " +
                    $"{request.TVShowEpisodeToInsertDto.EpisodeNumber} in the season number " +
                    $"{request.TVShowEpisodeToInsertDto.SeasonNumber} in the TV Show with id : " +
                    $"{request.TVShowEpisodeToInsertDto.TVShowId} is already exist!");
            }

            //episode file name "{TVShow Title}-{Season number}-{Episode number}" 
            // validate if the episode is already exist
            string pathOfTheTVShowDirectory = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                targetTVShow.Title);

            if(!Directory.Exists(pathOfTheTVShowDirectory))
            {
                throw new InsertionException($"The directory of the target TV Show with id {request.TVShowEpisodeToInsertDto.TVShowId} does not exist");
            }

            string episodeFilePath = Path.Combine(pathOfTheTVShowDirectory,
                $"{targetTVShow.Title}" +
                $"-{targetSeason.SeasonNumber}" +
                $"-{request.TVShowEpisodeToInsertDto.SeasonNumber}");
            
            if(File.Exists(episodeFilePath))
            {
                throw new InsertionException($"The episode of the TVShow with id : {targetTVShow.Id}" +
                    $" in season number {targetSeason.SeasonNumber} with episode number : {request.TVShowEpisodeToInsertDto.EpisodeNumber}" +
                    $" is already exist");
            }

            //create the file and save in the database:
            try
            {
                File.Copy(request.TVShowEpisodeToInsertDto.Location, episodeFilePath);
            }
            catch(Exception ex)
            {
                throw new InsertionException(ex.Message);
            }

            //save to the database:
            var episodeToInsert = request.TVShowEpisodeToInsertDto.Adapt<TVShowEpisode>();
            episodeToInsert.Location = episodeFilePath.Split('\\').Last();
            episodeToInsert.Location = Convert.ToBase64String(Encoding.UTF8.GetBytes(episodeToInsert.Location));

            try
            {
                await applicationDbContext.TVShowEpisodes.AddAsync(episodeToInsert);
                await applicationDbContext.SaveChangesAsync();

                return episodeToInsert.Adapt<TVShowEpisodeDto>();
            }
            catch(Exception ex)
            {
                throw new InsertionException($"{ex.Message}");
            }
        }
    }
}
