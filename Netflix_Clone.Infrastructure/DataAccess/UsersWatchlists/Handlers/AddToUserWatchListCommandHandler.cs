using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Handlers
{
    internal class AddToUserWatchListCommandHandler(ILogger<AddToUserWatchListCommandHandler> logger,
        ApplicationDbContext applicationDbContext) : IRequestHandler<AddToUserWatchListCommand, ApiResponseDto>
    {
        private readonly ILogger<AddToUserWatchListCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto> Handle(AddToUserWatchListCommand request, CancellationToken cancellationToken)
        {
            var targetContentToAdd = await applicationDbContext
                .Contents
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.contentId);

            if (targetContentToAdd is null)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Can not find the required content"
                };
            }

            var userWatchlist = applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x => x.WatchListContents)
                .AsSplitQuery()
                .SingleOrDefault(x => x.UserId == request.userId);

            if (userWatchlist is null)
            {
                //create a user watchlist
                userWatchlist = new UserWatchList
                {
                    UserId = request.userId,
                    WatchListContents = []
                };

                try
                {
                    await applicationDbContext.UsersWatchLists.AddAsync(userWatchlist);

                    await applicationDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ApiResponseDto
                    {
                        Result = null!,
                        Message = $"Can not create a watch list for the user with id {request.userId}"
                    };
                }
            }

            //is the content in the watch list 
            var IsTheContentInTheWatchList = await applicationDbContext
                .WatchListsContents
                .AsNoTracking()
                .AnyAsync(x => x.WatchListId == userWatchlist.Id && x.ContentId == request.contentId);

            if (IsTheContentInTheWatchList)
            {
                return new ApiResponseDto
                {
                    Result = new { },
                    Message = $"The content with id : {request.contentId} is already in the watchlist"
                };
            }

            try
            {
                userWatchlist.WatchListsContents.Add(new WatchListContent { ContentId = request.contentId, WatchListId = userWatchlist.Id });

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto
                {
                    Result = new { }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not add the content with id : {request.contentId} to the watch list"
                };
            }
        }
    }
}
