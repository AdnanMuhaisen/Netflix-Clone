namespace Netflix_Clone.Domain.Entities
{
    public class UserSubscriptionPlan
    {
        public int Id { get; set; }
        public string UserId { get; init; } = string.Empty;
        public int SubscriptionPlanId { get; init; }


        public ApplicationUser ApplicationUser { get; set; } = default!;
        public SubscriptionPlan SubscriptionPlan { get; set; } = default!;
    }
}
