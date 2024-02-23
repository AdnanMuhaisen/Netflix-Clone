using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Handlers
{
    public class AddNewUserSubscriptionCommandHandler(ILogger<AddNewUserSubscriptionCommandHandler> logger,
        ApplicationDbContext applicationDbContext) 
        : IRequestHandler<AddNewUserSubscriptionCommand, ApiResponseDto<UserSubscriptionPlanDto>>
    {
        private readonly ILogger<AddNewUserSubscriptionCommandHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<UserSubscriptionPlanDto>> Handle(AddNewUserSubscriptionCommand request, CancellationToken cancellationToken)
        {
            bool IsSubscriptionPlanExists = await applicationDbContext
                .SubscriptionPlans
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.planId);

            if (!IsSubscriptionPlanExists)
            {
                logger.LogError($"Can not find the subscription plan with id : {request.planId}");

                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = null!,
                    Message = $"Can not find the subscription plan with id : {request.planId}",
                    IsSucceed = false
                };
            }

            var doesTheUserHaveActiveSubscription = await applicationDbContext
                .UsersSubscriptions
                .AsNoTracking()
                .Where(x => x.UserId == request.userId && (x.EndDate <= DateTime.UtcNow || !x.IsEnded))
                .AnyAsync();

            if (doesTheUserHaveActiveSubscription)
            {
                logger.LogError($"The user with id : {request.userId} is already have an active subscription" +
                    $" plan");

                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = null!,
                    Message = $"The user is already have an active subscription",
                    IsSucceed = false
                };
            }

            var newUserSubscription = new UserSubscriptionPlan
            {
                UserId = request.userId,
                SubscriptionPlanId = request.planId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                IsEnded = false,
            };

            try
            {
                await applicationDbContext.UsersSubscriptions.AddAsync(newUserSubscription);

                await applicationDbContext.SaveChangesAsync();

                logger.LogInformation($"The subscription plan with id : {request.planId} is added to the " +
                    $"user with id : {request.userId}");

                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = newUserSubscription.Adapt<UserSubscriptionPlanDto>(),
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Can not add the subscription plan with id : {request.planId} to the " +
                    $"user with id : {request.userId} due to : {ex.Message}");

                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = null!,
                    Message = $"Can not add user subscription for the user with id : {request.userId}",
                    IsSucceed = false
                };
            }
        }
    }
}
