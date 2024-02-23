using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Handlers
{
    public class DeleteSeasonEpisodeCommandHandler(ILogger<DeleteSeasonEpisodeCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<DeleteSeasonEpisodeCommand,
            ApiResponseDto<DeletionResultDto>>
    {
        private readonly ILogger<DeleteSeasonEpisodeCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<DeletionResultDto>> Handle(DeleteSeasonEpisodeCommand request, CancellationToken cancellationToken)
        {
            var targetEpisodeToDelete = await applicationDbContext
                .TVShowEpisodes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.TVShowId == request.tVShowSeasonEpisodeToDeleteDto.TVShowID
                && x.SeasonId == request.tVShowSeasonEpisodeToDeleteDto.TVShowSeasonID
                && x.Id == request.tVShowSeasonEpisodeToDeleteDto.EpisodeID);

            if(targetEpisodeToDelete is null)
            {
                logger.LogInformation($"Can not find the target episode with id : {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID}" +
                    $" to delete");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false
                    },
                    Message = $"Can not find the episode with id {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID}",
                    IsSucceed = false
                };
            }

            var episodeTVShow = await applicationDbContext
                .TVShows
                .SingleAsync(x => x.Id == request.tVShowSeasonEpisodeToDeleteDto.TVShowID);

            var episodeSeason = await applicationDbContext
                .TVShowsSeasons
                .SingleAsync(x => x.Id == request.tVShowSeasonEpisodeToDeleteDto.TVShowSeasonID);

            string pathOfTheEpisodeSeasonDirectory = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                Encoding.UTF8.GetString(Convert.FromBase64String(episodeTVShow.Location)),
                Encoding.UTF8.GetString(Convert.FromBase64String(episodeSeason.DirectoryName)));

            if(!Directory.Exists(pathOfTheEpisodeSeasonDirectory))
            {
                logger.LogInformation($"Can not find the target episode season directory");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = "Can not find the TV Show Season Directory",
                    IsSucceed = false
                };
            }

            string pathOfTheTargetEpisodeFile = Path.Combine(pathOfTheEpisodeSeasonDirectory,
                Encoding.UTF8.GetString(Convert.FromBase64String(targetEpisodeToDelete.FileName)));

            if(!File.Exists(pathOfTheTargetEpisodeFile))
            {
                logger.LogInformation($"Can not find the target episode file with path {pathOfTheTargetEpisodeFile} to delete");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = "Can not find the TV Show Season Episode File",
                    IsSucceed = false
                };
            }

            //delete the episode file:
            try
            {
                logger.LogTrace($"Try to delete the episode file with path : {pathOfTheTargetEpisodeFile}");

                File.Delete(pathOfTheTargetEpisodeFile);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Can not delete the episode file with path : {pathOfTheTargetEpisodeFile} " +
                    $"due to : {ex.Message}");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = ex.Message,
                    IsSucceed = false
                };
            }

            //delete the episode from the database

            try
            {
                applicationDbContext.TVShowEpisodes.Remove(targetEpisodeToDelete);

                episodeTVShow.TotalNumberOfEpisodes--;
                episodeSeason.TotalNumberOfEpisodes--;

                await applicationDbContext.SaveChangesAsync();

                logger.LogInformation($"The target episode with episode id : {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID}" +
                    $" is deleted successfully");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = true
                    },
                    IsSucceed = true
                };
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Can not delete the episode with id : {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID}" +
                    $" due to : {ex.Message}");

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"The episode with id {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID} is deleted from the disk but does" +
                    $" not deleted from the database because this exception : " + ex.Message,
                    IsSucceed = false
                };
            }
        }
    }
}
