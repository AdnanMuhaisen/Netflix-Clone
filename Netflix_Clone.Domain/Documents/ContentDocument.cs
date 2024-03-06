using Nest;

namespace Netflix_Clone.Domain.Documents
{
    [ElasticsearchType(RelationName = nameof(ContentDocument))]
    public class ContentDocument : Document
    {
        [Text(Name = "title")]
        public string Title { get; init; } = string.Empty;

        [Number(NumberType.Integer,Name = "release_year")]
        public int ReleaseYear { get; init; }

        [Number(NumberType.Short,Name = "minimum_age_to_watch")]
        public int MinimumAgeToWatch { get; init; }

        [Text(Name = "synopsis")]
        public string Synopsis { get; init; } = string.Empty;

        [Keyword(Name = "rating")]
        public string Rating { get; init; } = string.Empty;

        [Number(NumberType.Integer,Name = "total_number_of_downloads")]
        public int TotalNumberOfDownloads { get; set; }

        [Boolean(Name = "is_available_to_download")]
        public bool IsAvailableToDownload { get; set; }

        [Keyword(Name = "location")]
        public string Location { get; set; } = string.Empty;

        [Number(NumberType.Integer,Name = "language_id")]
        public int LanguageId { get; set; }

        [Number(NumberType.Integer,Name = "content_genre_id")]
        public int ContentGenreId { get; set; }

        [Number(NumberType.Integer,Name = "director_id")]
        public int DirectorId { get; set; }
    }
}
