namespace Netflix_Clone.Domain.Entities
{
    public class SubscriptionPlan(int Id, string Plan,decimal Price)
    {
        public int Id { get; init; } = Id;
        public string Plan { get; init; } = Plan;
        public decimal Price { get; init; } = Price;


        public IEnumerable<SubscriptionPlanFeature> PlanFeatures { get; set; } = new List<SubscriptionPlanFeature>();
        public IEnumerable<SubscriptionPlanSubscriptionPlanFeature> SubscriptionPlansFeatures { get; set; } = new List<SubscriptionPlanSubscriptionPlanFeature>();
    }
}
