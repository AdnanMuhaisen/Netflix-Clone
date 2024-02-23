using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Common.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Common.Handlers
{
    public class AddToUserWatchHistoryCommandHandler(ILogger<AddToUserWatchHistoryCommandHandler> logger,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<AddToUserWatchHistoryCommand, ApiResponseDto<bool>>
    {
        private readonly ILogger<AddToUserWatchHistoryCommandHandler> logger = logger;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<ApiResponseDto<bool>> Handle(AddToUserWatchHistoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace($"Try to fin the content with id : {request.addToUserWatchHistoryRequestDto.ContentId}" +
                $" to add to the user watch list");
            bool IsTargetContentExist = unitOfWork
                .ContentRepository
                .GetAllAsNoTracking()
                .Any(x => x.Id == request.addToUserWatchHistoryRequestDto.ContentId);

            if (!IsTargetContentExist)
            {
                logger.LogInformation($"Can not find the content with id : {request.addToUserWatchHistoryRequestDto.ContentId}");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    Message = $"Can not Find the content with id {request.addToUserWatchHistoryRequestDto.ContentId}",
                    IsSucceed = false
                };
            }

            var historyRecordToAdd = request.addToUserWatchHistoryRequestDto.Adapt<UserWatchHistory>();

            try
            {
                await unitOfWork
                    .UserWatchHistoryRepository
                    .AddAsync(historyRecordToAdd);

                await unitOfWork.SaveChangesAsync();

                logger.LogInformation($"The record is saved in the user watch list for the user with " +
                    $"id : {request.addToUserWatchHistoryRequestDto.ApplicationUserId}");

                return new ApiResponseDto<bool>
                {
                    Result = true,
                    Message = string.Empty,
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Can not add the content with id : {request.addToUserWatchHistoryRequestDto.ContentId}" +
                    $"to the user watch list for the user with id : {request.addToUserWatchHistoryRequestDto.ApplicationUserId} " +
                    $"due to : {ex.Message}");

                return new ApiResponseDto<bool>
                {
                    Result = false,
                    Message = $"Can not add the user history",
                    IsSucceed = false
                };
            }
        }
    }
}
