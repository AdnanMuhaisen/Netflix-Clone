using Netflix_Clone.Domain.Enums;

namespace Netflix_Clone.Application.Services.Checkers
{
    public static class MediaFileExtensionChecker
    {
        public static bool IsValidFileExtension(string FileExtension)
        {
            foreach (var extension in Enum.GetValues(typeof(MediaFileExtensions)))
            {
                if (FileExtension.Trim('.').Equals(((MediaFileExtensions)extension).ToString()
                    , StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
