namespace Netflix_Clone.Domain.Entities
{
    // this entity is for conjunction table for the relationship between
    // the subscription plan entity and with the subscription plan feature entity

    public class SubscriptionPlanSubscriptionPlanFeature
    {
        public int Id { get; init; }

        //Foreign keys 
        public int SubscriptionPlanId { get; init; }
        public int SubscriptionPlanFeatureId { get; init; }

        public SubscriptionPlan SubscriptionPlan { get; set; } = default!;
        public SubscriptionPlanFeature SubscriptionPlanFeature { get; set; } = default!;
    }
}
