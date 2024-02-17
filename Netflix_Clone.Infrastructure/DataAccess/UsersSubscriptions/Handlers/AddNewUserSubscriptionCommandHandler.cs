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
                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = null!,
                    Message = $"Can not find the subscription plan with id : {request.planId}",
                    IsSucceed = true
                };
            }

            var doesTheUserHaveActiveSubscription = await applicationDbContext
                .UsersSubscriptions
                .AsNoTracking()
                .Where(x => x.UserId == request.userId && (x.EndDate <= DateTime.UtcNow || !x.IsEnded))
                .AnyAsync();

            if (doesTheUserHaveActiveSubscription)
            {
                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = null!,
                    Message = $"The user is already have an active subscription",
                    IsSucceed = true
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

                return new ApiResponseDto<UserSubscriptionPlanDto>
                {
                    Result = newUserSubscription.Adapt<UserSubscriptionPlanDto>(),
                    IsSucceed = true
                };
            }
            catch (Exception ex)
            {
                //log
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
