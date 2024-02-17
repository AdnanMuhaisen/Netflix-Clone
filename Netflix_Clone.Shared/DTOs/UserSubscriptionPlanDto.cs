namespace Netflix_Clone.Shared.DTOs
{
    public record UserSubscriptionPlanDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnded { get; set; }
        public bool IsPaid { get; set; } = false;
        public string UserId { get; init; } = string.Empty;
        public int SubscriptionPlanId { get; init; }
    }
}
