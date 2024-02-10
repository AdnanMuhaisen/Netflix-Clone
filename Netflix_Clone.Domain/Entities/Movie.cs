namespace Netflix_Clone.Domain.Entities
{
    public class Movie(int Id, string Title, int ReleaseYear, int MinimumAgeToWatch, string Synopsis, string Location,int LengthInMinutes)
        : Content(Id, Title, ReleaseYear, MinimumAgeToWatch, Synopsis, Location)
    {
        public int LengthInMinutes { get; set; } = LengthInMinutes;
    }
}
