using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands;
using Netflix_Clone.Shared.DTOs;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Handlers
{
    public class DeleteTVShowCommandHandler(ILogger<DeleteTVShowCommandHandler> logger,
        ApplicationDbContext applicationDbContext,
        IOptions<ContentTVShowOptions> options) : IRequestHandler<DeleteTVShowCommand, ApiResponseDto<DeletionResultDto>>
    {
        private readonly ILogger<DeleteTVShowCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options = options;

        public async Task<ApiResponseDto<DeletionResultDto>> Handle(DeleteTVShowCommand request, CancellationToken cancellationToken)
        {
            var targetTVShowToDelete = await applicationDbContext
                .TVShows
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.tVShowId);

            if(targetTVShowToDelete is null)
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"The target TV Show to delete with id {request.tVShowId} does not exist",
                    IsSucceed = false
                };
            }

            string pathOfTheTargetTVShow = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                (string.IsNullOrEmpty(targetTVShowToDelete.Location))
                ? $"{targetTVShowToDelete.Title}"
                : Encoding.UTF8.GetString(Convert.FromBase64String(targetTVShowToDelete.Location)));

            if(!Directory.Exists(pathOfTheTargetTVShow))
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"Can not find the target TV Show directory",
                    IsSucceed = false
                };
            }

            //delete
            try
            {
                Directory.Delete(pathOfTheTargetTVShow, true);
            }
            catch(Exception ex)
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = $"Can not delete the target TV Show directory",
                    IsSucceed = false
                };
            }

            //delete from the database:
            try
            {
                applicationDbContext
                    .TVShowEpisodes
                    .Where(x => x.TVShowId == request.tVShowId)
                    .ExecuteDelete();

                applicationDbContext.TVShows.Remove(targetTVShowToDelete);

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = true,
                    },
                    IsSucceed = true
                };
            }
            catch(Exception ex)
            {
                //log
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto
                    {
                        IsDeleted = false,
                    },
                    Message = "The TV Show is deleted from the disk but can not delete it from the database",
                    IsSucceed = false
                };
            }
        }
    }
}
