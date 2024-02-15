namespace Netflix_Clone.Domain.Entities
{
    public class Movie : Content
    { 
        public int LengthInMinutes { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Movie) return false;

            var targetObject = obj as Movie;

            return base.Equals(obj)
                && this.LengthInMinutes == targetObject!.LengthInMinutes;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * (23 + LengthInMinutes.GetHashCode());
        }
    }
}
