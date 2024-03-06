using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = nameof(PersonDocument))]
    public class PersonDocument : Document
    {
        [Text(Name = "first_name")]
        public string FirstName { get; init; } = string.Empty;

        [Text(Name = "last_name")]
        public string LastName { get; init; } = string.Empty;

        [Keyword(Name ="email")]
        public string? Email { get; init; }
    }
}
