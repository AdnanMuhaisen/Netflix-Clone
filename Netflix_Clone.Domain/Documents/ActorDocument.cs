using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = nameof(ActorDocument))]
    public class ActorDocument : PersonDocument
    {
    }
}
