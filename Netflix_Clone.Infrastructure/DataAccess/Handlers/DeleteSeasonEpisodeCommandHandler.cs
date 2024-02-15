using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DeleteSeasonEpisodeCommandHandler : IRequestHandler<DeleteSeasonEpisodeCommand, ApiResponseDto>
    {
        private readonly ILogger<DeleteSeasonEpisodeCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public DeleteSeasonEpisodeCommandHandler(ILogger<DeleteSeasonEpisodeCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }


        public async Task<ApiResponseDto> Handle(DeleteSeasonEpisodeCommand request, CancellationToken cancellationToken)
        {
            var targetEpisodeToDelete = await applicationDbContext
                .TVShowEpisodes
                .SingleOrDefaultAsync(x => x.TVShowId == request.tVShowSeasonEpisodeToDeleteDto.TVShowID
                && x.SeasonId == request.tVShowSeasonEpisodeToDeleteDto.TVShowSeasonID
                && x.Id == request.tVShowSeasonEpisodeToDeleteDto.EpisodeID);

            if(targetEpisodeToDelete is null)
            {
                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"Can not find the episode with id {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID}"
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
                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = "Can not find the TV Show Season Directory"
                };
            }

            string pathOfTheTargetEpisodeFile = Path.Combine(pathOfTheEpisodeSeasonDirectory,
                Encoding.UTF8.GetString(Convert.FromBase64String(targetEpisodeToDelete.FileName)));

            if(!File.Exists(pathOfTheTargetEpisodeFile))
            {
                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = "Can not find the TV Show Season Episode File"
                };
            }

            //delete the episode file:
            try
            {
                File.Delete(pathOfTheTargetEpisodeFile);
            }
            catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = ex.Message
                };
            }

            //delete the episode from the database

            try
            {
                applicationDbContext.TVShowEpisodes.Remove(targetEpisodeToDelete);

                episodeTVShow.TotalNumberOfEpisodes--;
                episodeSeason.TotalNumberOfEpisodes--;

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = true
                    }
                };
            }
            catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"The episode with id {request.tVShowSeasonEpisodeToDeleteDto.EpisodeID} is deleted from the disk but does" +
                    $" not deleted from the database because this exception : " + ex.Message
                };
            }
        }
    }
}
