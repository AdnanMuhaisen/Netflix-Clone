namespace Netflix_Clone.Domain.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; init; } 
        public string Plan { get; init; } = string.Empty;
        public decimal Price { get; init; } 


        public IEnumerable<SubscriptionPlanFeature> PlanFeatures { get; set; } = new List<SubscriptionPlanFeature>();
        public IEnumerable<SubscriptionPlanSubscriptionPlanFeature> SubscriptionPlansFeatures { get; set; } = new List<SubscriptionPlanSubscriptionPlanFeature>();

        public IEnumerable<ApplicationUser> PlanUsers { get; set; } = new List<ApplicationUser>();
        public IEnumerable<UserSubscriptionPlan> UsersSubscriptionPlans { get; set; } = new List<UserSubscriptionPlan>();
    }
}
