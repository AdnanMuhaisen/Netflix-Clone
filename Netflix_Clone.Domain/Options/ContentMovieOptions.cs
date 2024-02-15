namespace Netflix_Clone.Domain
{
    public class ContentMovieOptions
    {
        public string TargetDirectoryToCompressTo { get; set; } = string.Empty;
        public string TargetDirectoryToSaveTo { get; set; } = string.Empty;
        public string DefaultPathToDownload { get; set; } = string.Empty;
    }
}
