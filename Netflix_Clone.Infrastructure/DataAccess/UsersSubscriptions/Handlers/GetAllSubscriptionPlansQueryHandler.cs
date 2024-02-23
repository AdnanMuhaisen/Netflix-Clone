using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Queries;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Handlers
{
    public class GetAllSubscriptionPlansQueryHandler(ILogger<GetAllSubscriptionPlansQueryHandler> logger,
        ApplicationDbContext applicationDbContext) 
        : IRequestHandler<GetAllSubscriptionPlansQuery, ApiResponseDto<IEnumerable<SubscriptionPlanDto>>>
    {
        private readonly ILogger<GetAllSubscriptionPlansQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto<IEnumerable<SubscriptionPlanDto>>> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            var subscriptionPlans = await applicationDbContext
                .SubscriptionPlans
                .AsNoTracking()
                .ToListAsync();

            if (subscriptionPlans is null)
            {
                logger.LogInformation($"There are no subscription plans to retrieve");

                return new ApiResponseDto<IEnumerable<SubscriptionPlanDto>>
                {
                    Result = null!,
                    Message = "There`s no subscription plans",
                    IsSucceed = false
                };
            }

            var result = subscriptionPlans.Adapt<List<SubscriptionPlanDto>>();

            logger.LogInformation("The subscription plans are retrieved successfully");

            return new ApiResponseDto<IEnumerable<SubscriptionPlanDto>>
            {
                Result = result,
                IsSucceed = true
            };
        }
    }
}
