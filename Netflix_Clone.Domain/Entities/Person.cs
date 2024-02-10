namespace Netflix_Clone.Domain.Entities
{
    public abstract class Person(int Id,string FirstName,string LastName,string? Email = null,string? ImageUrl = null)
    {
        public int Id { get; init; } = Id;
        public string FirstName { get; init; } = FirstName;
        public string LastName { get; init; } = LastName;
        public string? Email { get; init; } = Email;
        public string? ImageUrl { get; init; } = ImageUrl;
    }
}
