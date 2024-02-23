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
        ApplicationDbContext applicationDbContext) 
        : IRequestHandler<AddToUserWatchListCommand, ApiResponseDto<bool>>
    {
        private readonly ILogger<AddToUserWatchListCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<bool>> Handle(AddToUserWatchListCommand request, CancellationToken cancellationToken)
        {
            var targetContentToAdd = await applicationDbContext
                .Contents
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.contentId);

            if (targetContentToAdd is null)
            {
                logger.LogError($"Can not find the content with id to add it to the user watch list " +
                    $"for the user with id : {request.userId}");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    Message = "Can not find the required content",
                    IsSucceed = false
                };
            }

            var userWatchlist = applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x => x.WatchListContents)
                .AsSplitQuery()
                .SingleOrDefault(x => x.UserId == request.userId);

            logger.LogTrace($"Try to get the user watch list for the user with id : {request.userId}");

            if (userWatchlist is null)
            {
                //create a user watchlist
                logger.LogInformation($"Create a user watch list for the user with id : {request.userId}");

                userWatchlist = new UserWatchList
                {
                    UserId = request.userId,
                    WatchListContents = []
                };

                try
                {
                    await applicationDbContext.UsersWatchLists.AddAsync(userWatchlist);

                    await applicationDbContext.SaveChangesAsync();

                    logger.LogInformation($"The user watch list for the user with id : {request.userId} " +
                        $"is created successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Can not create a user watch list for the user with id : {request.userId} " +
                        $"due to : {ex.Message}");

                    return new ApiResponseDto<bool>
                    {
                        Result = false,
                        Message = $"Can not create a watch list for the user with id {request.userId}",
                        IsSucceed = false
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
                logger.LogError($"The content with id : {request.contentId} is already in the user watch list " +
                    $"for the user with id : {request.userId}");

                return new ApiResponseDto<bool>
                {
                    Result = true,
                    Message = $"The content with id : {request.contentId} is already in the watchlist",
                    IsSucceed = false
                };
            }

            try
            {
                userWatchlist.WatchListsContents.Add(new WatchListContent { ContentId = request.contentId, WatchListId = userWatchlist.Id });

                await applicationDbContext.SaveChangesAsync();

                logger.LogInformation($"The content with content id : {request.contentId} is added to the user " +
                    $"watch list for the user with id : {request.userId}");

                return new ApiResponseDto<bool>
                {
                    Result = true,
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Can not add the content with id : {request.contentId} to the user watch list " +
                    $"for the user with id : {request.userId}");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    Message = $"Can not add the content with id : {request.contentId} to the watch list",
                    IsSucceed = false
                };
            }
        }
    }
}
