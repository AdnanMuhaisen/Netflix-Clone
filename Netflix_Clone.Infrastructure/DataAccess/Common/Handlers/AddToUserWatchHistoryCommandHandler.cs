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
            bool IsTargetContentExist = unitOfWork
                .ContentRepository
                .GetAllAsNoTracking()
                .Any(x => x.Id == request.addToUserWatchHistoryRequestDto.ContentId);

            if (!IsTargetContentExist)
            {
                return new ApiResponseDto<bool>
                {
                    Result = false,
                    Message = $"Can not Find the content with id {request.addToUserWatchHistoryRequestDto.ContentId}",
                    IsSucceed = true
                };
            }

            var historyRecordToAdd = request.addToUserWatchHistoryRequestDto.Adapt<UserWatchHistory>();

            try
            {
                await unitOfWork
                    .UserWatchHistoryRepository
                    .AddAsync(historyRecordToAdd);

                await unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Result = true,
                    Message = string.Empty,
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
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
