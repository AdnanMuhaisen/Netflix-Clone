using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = nameof(UserDocument))]
    public class UserDocument : Document
    {
        [Keyword(Name = "user_id")]
        public string UserId { get; set; } = string.Empty;

        [Text(Name = "first_name")]
        public string FirstName { get; init; } = string.Empty;

        [Text(Name = "last_name")]
        public string LastName { get; init; } = string.Empty;

        [Keyword(Name ="email")]
        public string? Email { get; init; }  

        [Keyword(Name ="phone_number")]
        public string? PhoneNumber { get; init; }
    }
}
