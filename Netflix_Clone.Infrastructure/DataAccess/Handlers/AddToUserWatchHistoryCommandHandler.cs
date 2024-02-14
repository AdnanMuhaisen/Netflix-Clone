using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class AddToUserWatchHistoryCommandHandler : IRequestHandler<AddToUserWatchHistoryCommand, ApiResponseDto>
    {
        private readonly ILogger<AddToUserWatchHistoryCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public AddToUserWatchHistoryCommandHandler(ILogger<AddToUserWatchHistoryCommandHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }



        public async Task<ApiResponseDto> Handle(AddToUserWatchHistoryCommand request, CancellationToken cancellationToken)
        {
            bool IsTargetContentExist = await applicationDbContext
                .Contents
                .AnyAsync(x => x.Id == request.addToUserWatchHistoryRequestDto.ContentId);

            if(!IsTargetContentExist)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not Find the content with id {request.addToUserWatchHistoryRequestDto.ContentId}"
                };
            }

            var historyRecordToAdd = request.addToUserWatchHistoryRequestDto.Adapt<UserWatchHistory>();

            try
            {
                await applicationDbContext
                    .UsersWatchHistories
                    .AddAsync(historyRecordToAdd);

                await applicationDbContext.SaveChangesAsync();

                return new ApiResponseDto
                {
                    Result = historyRecordToAdd.Adapt<UserWatchHistoryDto>()
                };
            }
            catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not add the user history"
                };
            }
        }
    }
}
