using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Common.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Common.Handlers
{
    public class AddToUserWatchHistoryCommandHandler(ILogger<AddToUserWatchHistoryCommandHandler> logger,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<AddToUserWatchHistoryCommand, ApiResponseDto>
    {
        private readonly ILogger<AddToUserWatchHistoryCommandHandler> logger = logger;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<ApiResponseDto> Handle(AddToUserWatchHistoryCommand request, CancellationToken cancellationToken)
        {
            bool IsTargetContentExist = unitOfWork
                .ContentRepository
                .GetAllAsNoTracking()
                .Any(x => x.Id == request.addToUserWatchHistoryRequestDto.ContentId);

            if (!IsTargetContentExist)
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
                await unitOfWork
                    .UserWatchHistoryRepository
                    .AddAsync(historyRecordToAdd);

                await unitOfWork.SaveChangesAsync();

                return new ApiResponseDto
                {
                    Result = historyRecordToAdd.Adapt<UserWatchHistoryDto>()
                };
            }
            catch (Exception ex)
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
