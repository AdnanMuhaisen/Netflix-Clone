using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = "movies_index")]
    public class MovieDocument : ContentDocument
    {
        [Number(NumberType.Integer,Name = "length_in_minutes")]
        public int LengthInMinutes { get; set; }
    }
}
