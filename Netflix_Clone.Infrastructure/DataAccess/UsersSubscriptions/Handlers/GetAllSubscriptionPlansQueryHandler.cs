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
        ApplicationDbContext applicationDbContext) : IRequestHandler<GetAllSubscriptionPlansQuery, ApiResponseDto>
    {
        private readonly ILogger<GetAllSubscriptionPlansQueryHandler> logger = logger;
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<ApiResponseDto> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            var subscriptionPlans = await applicationDbContext
                .SubscriptionPlans
                .AsNoTracking()
                .ToListAsync();

            if (subscriptionPlans is null)
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "There`s no subscription plans"
                };
            }

            var result = subscriptionPlans.Adapt<List<SubscriptionPlanDto>>();

            return new ApiResponseDto
            {
                Result = result
            };
        }
    }
}
