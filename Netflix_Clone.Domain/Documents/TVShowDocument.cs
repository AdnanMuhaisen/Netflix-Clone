using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = nameof(TVShowDocument))]
    public class TVShowDocument : ContentDocument
    {
        [Number(NumberType.Integer,Name = "total_number_of_seasons")]
        public int TotalNumberOfSeasons { get; set; }

        [Number(NumberType.Integer, Name = "total_number_of_episodes")]
        public int TotalNumberOfEpisodes { get; set; }
    }
}
