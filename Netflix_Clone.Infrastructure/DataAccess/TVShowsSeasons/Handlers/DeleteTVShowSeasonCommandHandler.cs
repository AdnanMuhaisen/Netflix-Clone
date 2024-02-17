using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Handlers
{
    public class DeleteTVShowSeasonCommandHandler(ILogger<DeleteTVShowSeasonCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<DeleteTVShowSeasonCommand, ApiResponseDto<DeletionResultDto>>
    {
        private readonly ILogger<DeleteTVShowSeasonCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<DeletionResultDto>> Handle(DeleteTVShowSeasonCommand request, CancellationToken cancellationToken)
        {
            var targetSeasonToDelete = await applicationDbContext
                .TVShowsSeasons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TVShowId == request.deleteTVShowSeasonRequestDto.TVShowId
                && (x.Id == request.deleteTVShowSeasonRequestDto.TVShowSeasonId
                    || x.SeasonNumber == request.deleteTVShowSeasonRequestDto.TVShowSeasonNumber));

            if(targetSeasonToDelete is null)
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false },
                    Message = $"The target season with id: {request.deleteTVShowSeasonRequestDto.TVShowSeasonId}," +
                    $" number: {request.deleteTVShowSeasonRequestDto.TVShowSeasonNumber} does not exist",
                    IsSucceed = true
                };
            }

            var seasonTVShow = await applicationDbContext
                .TVShows
                .SingleAsync(x => x.Id == request.deleteTVShowSeasonRequestDto.TVShowId);

            string pathOfTheSeasonDirectory = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                        Encoding.UTF8.GetString(Convert.FromBase64String(seasonTVShow.Location)),
                        Encoding.UTF8.GetString(Convert.FromBase64String(targetSeasonToDelete.DirectoryName)));

            if (!Directory.Exists(pathOfTheSeasonDirectory))
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false },
                    Message = $"The season directory does not exists",
                    IsSucceed = true
                };
            }

            //delete the season and the episodes of the season
            try
            {
                Directory.Delete(pathOfTheSeasonDirectory, recursive: true);
            }
            catch (Exception ex) 
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false },
                    Message = $"Can not delete the season directory because this error : {ex.Message}",
                    IsSucceed = false
                };
            }

            //delete from the database
            try
            {
               // delete the episodes of the season
                int numberOfDeletedEpisodes = await applicationDbContext
                    .TVShowEpisodes
                    .Where(x => x.TVShowId == request.deleteTVShowSeasonRequestDto.TVShowId
                    && (x.SeasonId == request.deleteTVShowSeasonRequestDto.TVShowSeasonId
                        || x.SeasonNumber == request.deleteTVShowSeasonRequestDto.TVShowSeasonNumber))
                    .ExecuteDeleteAsync();

                applicationDbContext
                    .TVShowsSeasons
                    .Remove(targetSeasonToDelete);

                seasonTVShow.TotalNumberOfSeasons--;
                seasonTVShow.TotalNumberOfEpisodes -= numberOfDeletedEpisodes;

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = true },
                    IsSucceed = true
                };
            }
            catch(Exception ex)
            {
                //log the error
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false },
                    Message = $"The season directory have been deleted but the records in the database does not deleted",
                    IsSucceed = false
                };
            }
        }
    }
}
