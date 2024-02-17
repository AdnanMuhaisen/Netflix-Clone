using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersSubscriptions.Commands
{
    public class AddNewUserSubscriptionCommand(string UserId, int PlanId) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
        public readonly int planId = PlanId;
    }
}
