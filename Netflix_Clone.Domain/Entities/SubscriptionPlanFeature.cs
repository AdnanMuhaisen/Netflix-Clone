namespace Netflix_Clone.Domain.Entities
{
    public class SubscriptionPlanFeature
    {
        public int Id { get; init; }
        public string Feature { get; init; } = string.Empty;

        public IEnumerable<SubscriptionPlan> FeaturePlans { get; set; } = new List<SubscriptionPlan>();
        public IEnumerable<SubscriptionPlanSubscriptionPlanFeature> SubscriptionPlansFeatures { get; set; } = new List<SubscriptionPlanSubscriptionPlanFeature>();
    }
}
