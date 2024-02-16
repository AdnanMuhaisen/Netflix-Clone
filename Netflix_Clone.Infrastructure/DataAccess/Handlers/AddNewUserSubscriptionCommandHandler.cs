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
    public class AddNewUserSubscriptionCommandHandler : IRequestHandler<AddNewUserSubscriptionCommand, ApiResponseDto>
    {
        private readonly ILogger<AddNewUserSubscriptionCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public AddNewUserSubscriptionCommandHandler(ILogger<AddNewUserSubscriptionCommandHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<ApiResponseDto> Handle(AddNewUserSubscriptionCommand request, CancellationToken cancellationToken)
        {
            bool IsSubscriptionPlanExists = await applicationDbContext
                .SubscriptionPlans
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.planId);

            if(!IsSubscriptionPlanExists)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not find the subscription plan with id : {request.planId}"
                };
            }

            var doesTheUserHaveActiveSubscription = await applicationDbContext
                .UsersSubscriptions
                .AsNoTracking()
                .Where(x => x.UserId == request.userId && (x.EndDate <= DateTime.UtcNow || !x.IsEnded))
                .AnyAsync();

            if(doesTheUserHaveActiveSubscription)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"The user is already have an active subscription"
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

                return new ApiResponseDto
                {
                    Result = newUserSubscription.Adapt<UserSubscriptionPlanDto>()
                };
            }
            catch (Exception ex)
            {
                //log
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = $"Can not add user subscription for the user with id : {request.userId}"
                };
            }
        }
    }
}
