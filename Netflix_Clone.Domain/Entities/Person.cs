namespace Netflix_Clone.Domain.Entities
{
    public abstract class Person
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? ImageUrl { get; init; } 
    }
}
