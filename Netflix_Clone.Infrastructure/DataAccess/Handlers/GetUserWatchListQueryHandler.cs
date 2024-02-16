using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetUserWatchListQueryHandler : IRequestHandler<GetUserWatchListQuery, ApiResponseDto>
    {
        private readonly ILogger<GetUserWatchListQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetUserWatchListQueryHandler(ILogger<GetUserWatchListQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<ApiResponseDto> Handle(GetUserWatchListQuery request, CancellationToken cancellationToken)
        {
            var userWatchList = await applicationDbContext
                .UsersWatchLists
                .AsNoTracking()
                .Include(x => x.WatchListContents)
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

                    return new ApiResponseDto
                    {
                        Result = userWatchList.Adapt<UserWatchListDto>()
                    };
                }
                catch (Exception ex)
                {
                    new ApiResponseDto
                    {
                        Result = null!,
                        Message = $"Can not create a watch list for the user with id {request.userId}"
                    };
                }
            }

            var result = userWatchList.Adapt<UserWatchListDto>();

            return new ApiResponseDto { Result = result };
        }
    }
}
