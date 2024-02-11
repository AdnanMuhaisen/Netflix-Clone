using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.Entities
{
    public abstract class Content
    {
        public int Id { get; set; }
        public string Title { get; init; } = string.Empty;
        public int ReleaseYear { get; init; }
        public int MinimumAgeToWatch { get; init; }
        public string Synopsis { get; init; } = string.Empty;
        public string Rating { get; init; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        //relationships

        public int LanguageId { get; set; }
        public ContentLanguage ContentLanguage { get; set; } = default!;

        public IEnumerable<Award> ContentAwards { get; set; } = new List<Award>();
        public IEnumerable<ContentAward> ContentsAwards { get; set; } = new List<ContentAward>();

        public int ContentGenreId { get; set; }
        //public ContentGenre Genre { get; set; }


        public int DirectorId { get; set; }
        //public Director Director { get; set; }

        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
        public IEnumerable<ContentActor> ContentsActors { get; set; } = new List<ContentActor>();

        public IEnumerable<ApplicationUser> WatchedBy { get; set; } = new List<ApplicationUser>();
        public IEnumerable<UserWatchHistory> UsersHistory { get; set; } = new List<UserWatchHistory>();

        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public IEnumerable<ContentTag> ContentsTags { get; set; } = new List<ContentTag>();
    }
}
