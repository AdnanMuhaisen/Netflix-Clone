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

        public int TotalNumberOfDownloads { get; set; }
        public bool IsAvailableToDownload { get; set; }


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


        public ICollection<ApplicationUser> DownloadedBy { get; set; } = new List<ApplicationUser>();
        public ICollection<ContentDownload> ContentsDownloads { get; set; } = new List<ContentDownload>();

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not Content) return false;
            if (ReferenceEquals(this, obj)) return true;

            var targetObject = obj as Content;

            return this.Id == targetObject!.Id
                && this.Title == targetObject.Title
                && this.ReleaseYear == targetObject.ReleaseYear
                && this.MinimumAgeToWatch == targetObject.MinimumAgeToWatch
                && this.Synopsis == targetObject.Synopsis
                && this.Rating == targetObject.Rating
                && this.TotalNumberOfDownloads == targetObject.TotalNumberOfDownloads
                && this.IsAvailableToDownload == targetObject.IsAvailableToDownload
                && this.Location == targetObject.Location;
        }

        public override int GetHashCode()
        {
            var hash = 7;
            hash *= 23 + Id.GetHashCode();
            hash *= 23 + Title.GetHashCode();
            hash *= 23 + ReleaseYear.GetHashCode();
            hash *= 23 + MinimumAgeToWatch.GetHashCode();
            hash *= 23 + Synopsis.GetHashCode();
            hash *= 23 + Rating.GetHashCode();
            hash *= 23 + TotalNumberOfDownloads.GetHashCode();
            hash *= 23 + IsAvailableToDownload.GetHashCode();
            hash *= 23 + Location.GetHashCode();
            
            return hash;
        }
    }
}
