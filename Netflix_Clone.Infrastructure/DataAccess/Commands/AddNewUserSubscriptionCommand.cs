using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewUserSubscriptionCommand(string UserId, int PlanId) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
        public readonly int planId = PlanId;
    }
}
