using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.Entities
{
    public abstract class Content(int Id,string Title,int ReleaseYear,int MinimumAgeToWatch, string Synopsis,string Location)
    {
        public int Id { get; init; } = Id;
        public string Title { get; init; } = Title;
        public int ReleaseYear { get; init; } = ReleaseYear;
        public int MinimumAgeToWatch { get; init; } = MinimumAgeToWatch;
        public string Synopsis { get; init; } = Synopsis;
        public string Rating { get; } = $"Recommended for ages {MinimumAgeToWatch} and up";

        [Url]
        public string Location { get; init; } = Location;

        //relationships

        public int LanguageId { get; set; }
        public ContentLanguage ContentLanguage { get; set; } = default!;

        public IEnumerable<Award> ContentAwards { get; set; } = new List<Award>();
        public IEnumerable<ContentAward> ContentsAwards { get; set; } = new List<ContentAward>();

        public int GenreId { get; set; }
        public ContentGenre Genre { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }

        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
        public IEnumerable<ContentActor> ContentsActors { get; set; } = new List<ContentActor>();
    }
}
