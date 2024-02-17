namespace Netflix_Clone.Shared.DTOs
{
    public class SubscriptionPlanDto
    {
        public int Id { get; init; }
        public string Plan { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}
