using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Queries
{
    public class GetAllSubscriptionPlansQuery
        : IRequest<ApiResponseDto<IEnumerable<SubscriptionPlanDto>>>
    {

    }
}
