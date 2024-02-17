using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Queries;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Handlers
{
    public class GetUserWatchListQueryHandler(ILogger<GetUserWatchListQueryHandler> logger,
        ApplicationDbContext applicationDbContext) : IRequestHandler<GetUserWatchListQuery, ApiResponseDto<UserWatchListDto>>
    {
        private readonly ILogger<GetUserWatchListQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<UserWatchListDto>> Handle(GetUserWatchListQuery request, CancellationToken cancellationToken)
        {
            var userWatchList = await applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x => x.WatchListContents)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.UserId == request.userId);

            if (userWatchList is null)
            {
                //create a user watchlist
                userWatchList = new UserWatchList
                {
                    UserId = request.userId
                };
                try
                {
                    await applicationDbContext.UsersWatchLists.AddAsync(userWatchList);

                    await applicationDbContext.SaveChangesAsync();

                    return new ApiResponseDto<UserWatchListDto>
                    {
                        Result = userWatchList.Adapt<UserWatchListDto>(),
                        IsSucceed = true
                    };
                }
                catch (Exception ex)
                {
                    return new ApiResponseDto<UserWatchListDto>
                    {
                        Result = null!,
                        Message = $"Can not create a watch list for the user with id {request.userId}",
                        IsSucceed = false
                    };
                }
            }

            var result = userWatchList.Adapt<UserWatchListDto>();

            return new ApiResponseDto<UserWatchListDto>
            {
                Result = result,
                IsSucceed = true
            };
        }
    }
}
