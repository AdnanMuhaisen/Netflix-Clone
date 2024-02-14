using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetAllSubscriptionPlansQueryHandler : IRequestHandler<GetAllSubscriptionPlansQuery, ApiResponseDto>
    {
        private readonly ILogger<GetAllSubscriptionPlansQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetAllSubscriptionPlansQueryHandler(ILogger<GetAllSubscriptionPlansQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<ApiResponseDto> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            var subscriptionPlans = await applicationDbContext
                .SubscriptionPlans
                .ToListAsync();

            if(subscriptionPlans is null)
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
