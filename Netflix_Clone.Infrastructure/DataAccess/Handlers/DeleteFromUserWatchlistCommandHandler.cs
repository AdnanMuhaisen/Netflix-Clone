using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DeleteFromUserWatchlistCommandHandler : IRequestHandler<DeleteFromUserWatchListCommand, ApiResponseDto>
    {
        private readonly ILogger<DeleteFromUserWatchlistCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public DeleteFromUserWatchlistCommandHandler(ILogger<DeleteFromUserWatchlistCommandHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<ApiResponseDto> Handle(DeleteFromUserWatchListCommand request, CancellationToken cancellationToken)
        {
            var targetWatchList = await applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x=>x.WatchListContents)
                .SingleOrDefaultAsync(x => x.UserId == request.userId);

            if(targetWatchList is null)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not find the user watch list !"
                };
            }

            if(!targetWatchList.WatchListContents.Any(x=>x.Id == request.contentId))
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not find the content with id : {request.contentId} in the user watchlist !"
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

                return new ApiResponseDto
                {
                    Result = new { }
                };
            }
            catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not delete the content with id : {request.contentId} from the watchlist"
                };
            }
        }
    }
}
