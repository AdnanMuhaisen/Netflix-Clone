namespace Netflix_Clone.Domain.Entities
{
    public class SubscriptionPlanFeature(int Id,string Feature)
    {
        public int Id { get; init; } = Id;
        public string Feature { get; init; } = Feature;

        public IEnumerable<SubscriptionPlan> FeaturePlans { get; set; } = new List<SubscriptionPlan>();
        public IEnumerable<SubscriptionPlanSubscriptionPlanFeature> SubscriptionPlansFeatures { get; set; } = new List<SubscriptionPlanSubscriptionPlanFeature>();
    }
}
