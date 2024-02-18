using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Handlers
{
    public class DeleteFromUserWatchlistCommandHandler(ILogger<DeleteFromUserWatchlistCommandHandler> logger,
        ApplicationDbContext applicationDbContext)
        : IRequestHandler<DeleteFromUserWatchListCommand, ApiResponseDto<DeletionResultDto>>
    {
        private readonly ILogger<DeleteFromUserWatchlistCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<DeletionResultDto>> Handle(DeleteFromUserWatchListCommand request, CancellationToken cancellationToken)
        {
            var targetWatchList = await applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x => x.WatchListContents)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.UserId == request.userId);

            if (targetWatchList is null)
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false },
                    Message = "Can not find the user watch list !",
                    IsSucceed = false
                };
            }

            if (!targetWatchList.WatchListContents.Any(x => x.Id == request.contentId))
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted = false},
                    Message = $"Can not find the content with id : {request.contentId} in the user watchlist !",
                    IsSucceed = false
                };
            }

            //delete the content from the watchlist
            try
            {
                applicationDbContext
                    .WatchListsContents
                    .Where(x => x.WatchListId == targetWatchList.Id && x.ContentId == request.contentId)
                    .ExecuteDelete();

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto{IsDeleted=true },
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<DeletionResultDto>
                {
                    Result = new DeletionResultDto { IsDeleted=false},
                    Message = $"Can not delete the content with id : {request.contentId} from the watchlist",
                    IsSucceed = false
                };
            }
        }
    }
}
